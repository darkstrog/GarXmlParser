using GARReplication.Core;
using Microsoft.Extensions.Configuration;

namespace GARReplication.Settings
{
    public static class ReplicationSettingsFactory
    {
        public static ReplicationSettings FromConfiguration(IConfiguration? config = null)
        {
            return new ReplicationSettings
            {
                DataPath = config?["SourceSettings:ZipSourcePath"] ?? string.Empty,
                Regions = config?.GetSection("SourceSettings:Regions").Get<int[]>(),
                BatchSize = config?.GetValue<int>("ReplicationSettings:BatchSize") ?? 10000,
                Strategy = config?["ReplicationSettings:Strategy"] ?? "fullrepl",
                ConnectionString = config?.GetConnectionString("DefaultConnection") ?? string.Empty
            };
        }
    }

    public static class ReplicationSettingsExtensions
    {
        public static ReplicationSettings WithCommandLineOverrides(
            this ReplicationSettings settings,
            string? dataPath = null,
            int[]? regions = null,
            int? batchSize = null,
            string? strategy = null,
            string? connectionString = null)
        {
            return settings with
            {
                DataPath = dataPath ?? settings.DataPath,
                Regions = regions?.Length > 0 ? regions : settings.Regions,
                BatchSize = batchSize ?? settings.BatchSize,
                Strategy = strategy ?? settings.Strategy,
                ConnectionString = connectionString ?? settings.ConnectionString
            };
        }
    }
}
