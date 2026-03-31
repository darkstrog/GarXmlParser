using Microsoft.Extensions.Configuration;

namespace GARReplication.Commands
{
    internal class ConfigDisplayHelper
    {
        internal static void ShowConfiguration()
        {
            var config = LoadConfiguration();

            Console.WriteLine("\nТекущие настройки приложения:");
            Console.WriteLine(new string('=', 50));

            DisplaySection(config.GetSection("ReplicationSettings"), "ReplicationSettings");
            DisplaySection(config.GetSection("SourceSettings"), "SourceSettings");
            DisplayConnectionStrings(config.GetSection("ConnectionStrings"));
        }

        private static IConfiguration LoadConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

        private static void DisplaySection(IConfigurationSection section, string title)
        {
            Console.WriteLine($"\n[{title}]");
            foreach (var child in section.GetChildren())
            {
                if (child.GetChildren().Any())
                {
                    Console.Write($"  {child.Key}:");
                    DisplayChildSection(child, "    ");
                }
                else
                {
                    Console.WriteLine($"  {child.Key}: {child.Value}");
                }

            }
        }

        private static void DisplayChildSection(IConfigurationSection section, string title)
        {

            foreach (var child in section.GetChildren())
            {
                if (child.GetChildren().Any())
                {
                    Console.Write($"{title}{child.Key}: ");
                    DisplayChildSection(child, title + "    ");
                }
                else
                {
                    Console.Write($"{child.Value} ");
                }
            }
            Console.WriteLine("");
        }

        private static void DisplayConnectionStrings(IConfigurationSection section)
        {
            Console.WriteLine("\n[ConnectionStrings]");
            foreach (var child in section.GetChildren())
            {
                Console.WriteLine($"  {child.Key}: {MaskConnectionString(child.Value)}");
            }
        }

        internal static string MaskConnectionString(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) return "не указана";

            return connectionString.Contains("Password=", StringComparison.OrdinalIgnoreCase)
                ? System.Text.RegularExpressions.Regex.Replace(connectionString, "(Password=)([^;]+)", "$1*****")
                : connectionString;
        }
    }
}
