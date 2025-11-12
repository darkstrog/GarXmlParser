using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Interfaces.Services;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;

namespace GAROperations.Infrastructure.Services
{
    public class AddressObjectService : IAddressObjectService
    {
        private readonly ILogger<AddressObjectService>? _logger;
        private readonly IAddressObjectRepo _repository;
        private readonly IProblemDataService<AddressObjectDto> _problemDataService;
        public AddressObjectService(IAddressObjectRepo repo, IProblemDataService<AddressObjectDto> problemDataService, ILogger<AddressObjectService> logger)
        {
            _logger = logger;
            _repository = repo;
            _problemDataService = problemDataService;
        }
        public async Task InsertDataAsync(AddressObjectDto addressObject)
        {
            try
            {
                _logger?.LogInformation("Добавление одной записи AddressObject с OBJECTID: {OBJECTID} в базу", addressObject.OBJECTID);
                await _repository.InsertAsync(addressObject);
            }
            catch (Exception)
            {
                _logger?.LogError("Ошибка При добавлении записи AddressObject с OBJECTID: {OBJECTID}", addressObject.OBJECTID);
                throw;
            }
        }
        /// <summary>
        /// Метод для массовой асинхронной заливки объектов из асинхронного перечисляемого источника
        /// </summary>
        /// <param name="addressObjects"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task InsertDataBulkAsync(IAsyncEnumerable<AddressObjectDto> addressObjects, int batchSize = 1000)
        {
            var batch = new List<AddressObjectDto>(batchSize);

            await foreach (var item in addressObjects)
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    await ProcessBatchWithFallback(batch, batchSize);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                await ProcessBatchWithFallback(batch, batchSize);
            }
        }
        /// <summary>
        /// Пакетная заливка в базу с откатом и дроблением пакета для последующих повторных попыток  в случае неудачи
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="originalBatchSize"></param>
        /// <returns></returns>
        private async Task ProcessBatchWithFallback(List<AddressObjectDto> batch, int originalBatchSize)
        {
            try
            {
                await _repository.InsertBulkAsync(batch);
                _logger?.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Ошибка при обработке пачки из {Count} записей, дробим на более мелкие", batch.Count);

                await InsertAddressObjectsBulk(batch, Math.Max(1, originalBatchSize / 10));
            }
        }
        /// <summary>
        /// Метод для массовой залики объектов в базу 
        /// </summary>
        /// <param name="addressObjects"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task InsertAddressObjectsBulk(IEnumerable<AddressObjectDto> addressObjects, int batchSize = 100)
        {
            var batch = new List<AddressObjectDto>(batchSize);

            foreach (var item in addressObjects)
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    await ProcessSingleBatchWithIsolation(batch);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
            {
                await ProcessSingleBatchWithIsolation(batch);
            }
        }
        /// <summary>
        /// Метод отправляет один пакет объектов в базу и в случае неудачи пытается отправить объекты по одному,
        /// при неудачной попытке отправляет запись в обработчик проблемных записей
        /// </summary>
        /// <param name="batch"></param>
        /// <returns></returns>
        private async Task ProcessSingleBatchWithIsolation(List<AddressObjectDto> batch)
        {
            
            if (batch.Count == 1)
            {
                await HandleProblematicRecord(batch[0]);
                return;
            }

            try
            {
                await _repository.InsertBulkAsync(batch);
                _logger?.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Ошибка при обработке пачки из {Count} записей, обрабатываем по одной", batch.Count);

                foreach (var record in batch)
                {
                    try
                    {
                        await _repository.InsertAsync(record);
                    }
                    catch (Exception singleEx)
                    {
                        _logger?.LogError(singleEx, "Не удалось записать в базу {RecordId} ,отправляем в сервис обработки проблемных данных", record.ID);
                        await HandleProblematicRecord(record);
                    }
                }
            }
        }
        /// <summary>
        /// Обработчик записей которые неудалось записать в бд
        /// </summary>
        /// <param name="problematicRecord"></param>
        /// <returns></returns>
        private async Task HandleProblematicRecord(AddressObjectDto problematicRecord)
        {
            try
            {
                await _problemDataService.ProcessProblematicRecord(problematicRecord, null);
                _logger?.LogInformation("Проблемная запись {RecordId} отправлена на анализ", problematicRecord.ID);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Не удалось отправить проблемную запись {RecordId} в сервис обработки", problematicRecord.ID);
                await WriteToCsvFallback(problematicRecord);
            }
        }
        /// <summary>
        /// Метод для логирования проблемных записей в случае недоступности сервиса обработки проблемных записей
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private async Task WriteToCsvFallback(AddressObjectDto record)
        {
            var csvLine = $"{DateTime.Now},{record.ID},{record.OBJECTID},{record.NAME?.Replace(",", ";")}";
            await File.AppendAllTextAsync($"problematic_records{DateTime.Today}.csv", csvLine + Environment.NewLine);
            _logger?.LogInformation("Информация о проблемной записи добавлена в файл: problematic_records.csv");
        }

    }
}
