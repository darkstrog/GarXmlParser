using GAROperations.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GAROperations.Infrastructure.Services
{
    /// <summary>
    /// Сервис предназначен для логирования записей которые по какой то причине неудалось записать в базу
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonProblemDataService<T> : IProblemDataService<T> where T : class
    {
        private readonly string _filePath;
        private readonly ILogger<JsonProblemDataService<T>> _logger;
        private static readonly object _fileLock = new object();
        private static bool _isFileInitialized = false;
        private readonly JsonSerializerOptions _jsonSettings = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public JsonProblemDataService(ILogger<JsonProblemDataService<T>> logger)
        {
            _logger = logger;
            var typeName = typeof(T).Name.ToLower();
            _filePath = $"problematic_{typeName}_{DateTime.Now:yyyyMMdd}.jsonl";
            InitializeFile();
        }

        private void InitializeFile()
        {
            lock (_fileLock)
            {
                if (!_isFileInitialized && !File.Exists(_filePath))
                {
                    var header = $"# Проблемные записи {typeof(T).Name} - {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                    File.WriteAllText(_filePath, header + Environment.NewLine);
                    _isFileInitialized = true;
                }
            }
        }

        public Task ProcessProblematicRecord(T record, string? error = null)
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                RecordType = typeof(T).Name,
                Record = record,
                Error = error
            };

            var jsonString = JsonSerializer.Serialize(logEntry, _jsonSettings);

            lock (_fileLock)
            {
                File.AppendAllText(_filePath, jsonString + Environment.NewLine);
            }

            _logger.LogWarning("Проблемная запись {Type} записана в JSON", typeof(T).Name);

            return Task.CompletedTask;
        }
    }
}
