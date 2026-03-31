using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace GARReplication.Commands
{
    internal class SetConfigCommand
    {
        internal static Command Create(IConfigurationRoot configRoot)
        {
            var command = new Command("set", "Обновляет настройки.");

            var strategyOption = new Option<string>(name: "--strategy")
            {
                Description = "Стратегия репликации по умолчанию (fullrepl)",
                Required = false,
            };

            var pathOption = new Option<string>("--path", "-p")
            {
                Description = "Путь к XML файлу или дирректории",
                Required = false
            };

            var batchOption = new Option<int>("--batch", "-b")
            {
                Description = "Размер пачки для вставки в базу одной транзакцией",
                Required = false
            };

            var regionOption = new Option<int[]?>("--region", "-r")
            {
                Description = "Перечень кодов регионов для репликации. 0 - все регионы",
                AllowMultipleArgumentsPerToken = true,
                Required = false
            };

            var connectionStringOption = new Option<string>("--conns", "-cs")
            {
                Description = "Строка подключения к базе данных",
                Required = false
            };

            command.Add(strategyOption);
            command.Add(pathOption);
            command.Add(batchOption);
            command.Add(regionOption);
            command.Add(connectionStringOption);

            command.SetAction(parseResult =>
            {
                string? dataPath = parseResult.GetValue<string>(pathOption);
                int[]? regions = parseResult.GetValue<int[]?>(regionOption);
                int? batchSize = parseResult.GetValue<int>(batchOption);
                string? strategy = parseResult.GetValue<string>(strategyOption);
                string? connectionString = parseResult.GetValue<string>(connectionStringOption);

                if (strategy != null)
                    configRoot["ReplicationSettings:Strategy"] = strategy;
                if (dataPath != null)
                    configRoot["SourceSettings:ZipSourcePath"] = dataPath;
                if (batchSize != null)
                    configRoot["SourceSettings:BatchSize"] = batchSize.ToString();
                if (connectionString != null)
                    configRoot["ConnectionStrings:DefaultConnection"] = connectionString;

                if (regions != null || regions?.Length == 0)
                {
                    if (regions.Length == 1 && regions[0] == 0)
                    {
                        configRoot["SourceSettings:Regions"] = null;
                    }
                    else
                    {
                        configRoot["SourceSettings:Regions"] =
                            System.Text.Json.JsonSerializer.Serialize(regions);
                    }
                }

                configRoot.SaveJsonProvider();
                Console.WriteLine("Настройки успешно применены. \nТекущие настройки:");
                ConfigDisplayHelper.ShowConfiguration();
            });
            return command;
        }
    }
}
