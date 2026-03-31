using Microsoft.Extensions.Configuration;

namespace GARReplication.Core
{
    /// <summary>
    /// Класс настроек репликатора.
    /// </summary>
    public record ReplicationSettings
    {
        public required string DataPath { get; init; }
        public int[]? Regions { get; init; }
        public int BatchSize { get; init; }
        public required string Strategy { get; init; }
        public required string ConnectionString { get; init; }
    }
}
