using GarXmlParser.Mappers;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace GarXmlParser
{
    internal class GarNodeParser
    {
    private readonly ILogger _logger;

    public GarNodeParser(ILogger<GarNodeParser> logger = null)
    {
        _logger = logger;
    }
    public async IAsyncEnumerable<T> GetXmlObjectAsync<T>(
        string filePath,
        IGarItemMapper<T> mapper,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(filePath)) { throw new ArgumentNullException(filePath); }
        ArgumentNullException.ThrowIfNull(mapper);

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            Async = true
        };

        using (var reader = XmlReader.Create(filePath, settings))
        {
            _logger?.LogInformation($"Начинается обработка: {filePath}");
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (reader.NodeType == XmlNodeType.Element && reader.Name == mapper.NodeName)
                {
                    using (var subtree = reader.ReadSubtree())
                    {
                        await subtree.ReadAsync().ConfigureAwait(false);
                        var element = await XElement.LoadAsync(subtree, LoadOptions.None, cancellationToken).ConfigureAwait(false);
                        if (element != null)
                        {
                            var item = mapper.GetFromXelement(element);
                            yield return item;
                        }
                    }
                }
            }
        }
    }
    public async IAsyncEnumerable<T> GetXmlObjectFromXMLContentAsync<T>
        (
            string xmlContent,
            IGarItemMapper<T> mapper,
            string fileName = "",
            [EnumeratorCancellation] CancellationToken cancellationToken = default
        )
    {
        if (string.IsNullOrEmpty(xmlContent)) { throw new ArgumentNullException(xmlContent); }
        ArgumentNullException.ThrowIfNull(mapper);

        var settings = new XmlReaderSettings
        {
            IgnoreComments = true,
            IgnoreWhitespace = true,
            Async = true
        };

        using (var xmlString = new StringReader(xmlContent))
        {
            using (var reader = XmlReader.Create(xmlString, settings))
            {
                _logger?.LogInformation($"Начинается обработка: {fileName}");
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == mapper.NodeName)
                    {
                        using (var subtree = reader.ReadSubtree())
                        {
                            await subtree.ReadAsync().ConfigureAwait(false);
                            var element = await XElement.LoadAsync(subtree, LoadOptions.None, cancellationToken).ConfigureAwait(false);
                            if (element != null)
                            {
                                var item = mapper.GetFromXelement(element);
                                yield return item;
                            }
                        }
                    }
                }
            }
        }
    }

}
}
