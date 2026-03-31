using Microsoft.Extensions.Configuration;
using System.CommandLine;

namespace GARReplication.Commands
{
    internal class ConfigCommand
    {
        internal static Command Create(IConfigurationRoot config)
        {
            var command = new Command("config", "Управление настройками");

            command.Add(ShowConfigCommand.Create());
            command.Add(SetConfigCommand.Create(config));

            return command;
        }
    }
}
