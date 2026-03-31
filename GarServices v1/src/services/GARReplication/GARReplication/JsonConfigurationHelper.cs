using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GARReplication
{
    //решение взято отсюда https://stackoverflow.com/a/79328423
    /// <summary>
    /// Хелпер для изменения конфигурации appsettings
    /// </summary>
    public static class JsonConfigurationHelper
    {
        public static bool SaveJsonProvider(this IConfigurationRoot configurationRoot)
        {
            //try to get the last registered json provider
            var provider = configurationRoot.Providers.LastOrDefault(p => p is JsonConfigurationProvider) as JsonConfigurationProvider;
            if (provider == null)
                return false;

            var filepath = Filepath(provider);
            if (filepath == null)
                return false;

            string? sJson = File.Exists(filepath)
              ? File.ReadAllText(filepath)
              : null;

            var rootNode = sJson != null
              ? JsonNode.Parse(sJson, documentOptions: new JsonDocumentOptions() { CommentHandling = JsonCommentHandling.Skip })
              : new JsonObject();

            var rootObj = rootNode?.AsObject();
            if (rootObj == null)
                return false;

            var earlierKeys = Enumerable.Empty<string>();

            void HandleKeys(JsonObject parentNode, string? parentPath)
            {
                foreach (var key in provider.GetChildKeys(earlierKeys, parentPath).Distinct())
                {
                    var fullKey = parentPath != null
                      ? ConfigurationPath.Combine(parentPath, key)
                      : key;
                    if (provider.TryGet(fullKey, out var sValue))
                    {
                        var jsonValue = JsonValue.Create(sValue);
                        parentNode[key] = jsonValue;
                    }
                    else
                    {
                        //probably a section
                        var node = parentNode[key];
                        var sectionObj = node == null
                          ? (parentNode[key] = new JsonObject()).AsObject()
                          : node.AsObject(); //this will throw if the node is not an object (e.g. there is a value with that key)
                        HandleKeys(sectionObj, fullKey);
                    }
                }
            }

            HandleKeys(rootObj, null);

            //save the json to file
            var options = new JsonSerializerOptions() { WriteIndented = true};
            sJson = rootNode?.ToJsonString(options);
            File.WriteAllText(filepath, sJson);
            return true;
        }

        internal static string? Filepath(FileConfigurationProvider fileConfigurationProvider)
        {
            IFileInfo? file = fileConfigurationProvider.Source.FileProvider?.GetFileInfo(fileConfigurationProvider.Source.Path ?? string.Empty);
            if (file == null)
                return null;
            return file.PhysicalPath;
        }
    }
}

