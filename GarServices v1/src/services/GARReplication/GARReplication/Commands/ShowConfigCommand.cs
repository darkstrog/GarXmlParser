using System.CommandLine;

namespace GARReplication.Commands
{
    internal class ShowConfigCommand
    {
        internal static Command Create()
        {
            var command = new Command("show", "Показать текущую конфигурацию");

            command.SetAction(_ =>
            {
                ConfigDisplayHelper.ShowConfiguration();
                return Task.CompletedTask;
            });

            return command;
        }
    }
}
