using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace GARReplication.Commands
{
    internal static class CommandLineArgsParser
    {
        internal static async Task ParseAsync(string[] args)
        {

            IConfigurationRoot configRoot = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .Build();


            var rootCommand = new RootCommand("Утилита репликации ГАР");

            rootCommand.Add(StartCommand.Create());
            rootCommand.Add(ConfigCommand.Create(configRoot));
            //TODO: rootCommand.Add(StrategyCommand.Create()); Добавить как минимум справку по "стратегиям"
            //на будущее возможно добавить возможность создавать их и коммандной строки
            await rootCommand.Parse(args).InvokeAsync();
        }
    }
        
}
