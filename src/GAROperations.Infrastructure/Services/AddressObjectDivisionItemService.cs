using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Interfaces.Services;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;

namespace GAROperations.Infrastructure.Services
{
    public class AddressObjectDivisionItemService: IAddressObjectDivisionItemService
    {
        private readonly ILogger<AddressObjectDivisionItemService> _logger;
        private readonly IAddressObjectDivisionItemRepo _repository;
        private readonly IProblemDataService<AddressObjectDivisionItemDto> _problemDataService;
        public AddressObjectDivisionItemService(IAddressObjectDivisionItemRepo repo, 
                                                IProblemDataService<AddressObjectDivisionItemDto> problemDataService, 
                                                ILogger<AddressObjectDivisionItemService> logger)
        {
            _logger = logger;
            _repository = repo;
            _problemDataService = problemDataService;
        }
        public async Task InsertDataAsync(AddressObjectDivisionItemDto entity)
        {
            try
            {
                _logger?.LogInformation("Добавление одной записи AddressObject с OBJECTID: {OBJECTID} в базу", entity.ID);
                await _repository.InsertAsync(entity);
            }
            catch (Exception)
            {
                _logger?.LogError("Ошибка При добавлении записи AddressObject с OBJECTID: {OBJECTID}", entity.ID);
                throw;
            }
        }
        /// <summary>
        /// Метод для массовой асинхронной заливки объектов из асинхронного перечисляемого источника
        /// </summary>
        /// <param name="addressObjects"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task InsertDataBulkAsync(IAsyncEnumerable<AddressObjectDivisionItemDto> entityes, int batchSize = 1000)
        {
            var batch = new List<AddressObjectDivisionItemDto>(batchSize);

            await foreach (var item in entityes)
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
        private async Task ProcessBatchWithFallback(List<AddressObjectDivisionItemDto> batch, int originalBatchSize)
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
        public async Task InsertAddressObjectsBulk(IEnumerable<AddressObjectDivisionItemDto> addressObjects, int batchSize = 100)
        {
            var batch = new List<AddressObjectDivisionItemDto>(batchSize);

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
        private async Task ProcessSingleBatchWithIsolation(List<AddressObjectDivisionItemDto> batch)
        {

            if (batch.Count == 1)
            {
                await HandleProblematicRecord(batch[0]);
                return;
            }

            try
            {
                await _repository.InsertBulkAsync(batch);
                _logger.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Ошибка при обработке пачки из {Count} записей, обрабатываем по одной", batch.Count);

                foreach (var record in batch)
                {
                    try
                    {
                        await _repository.InsertAsync(record);
                    }
                    catch (Exception singleEx)
                    {
                        _logger.LogError(singleEx, "Не удалось записать в базу {RecordId} ,отправляем в сервис обработки проблемных данных", record.ID);
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
        private async Task HandleProblematicRecord(AddressObjectDivisionItemDto problematicRecord)
        {
            try
            {
                await _problemDataService.ProcessProblematicRecord(problematicRecord, null);
                _logger.LogInformation("Проблемная запись {RecordId} отправлена на анализ", problematicRecord.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить проблемную запись {RecordId} в сервис обработки", problematicRecord.ID);
                await WriteToCsvFallback(problematicRecord);
            }
        }
        /// <summary>
        /// Метод для логирования проблемных записей в случае недоступности сервиса обработки проблемных записей
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private async Task WriteToCsvFallback(AddressObjectDivisionItemDto record)
        {
            var csvLine = $"{DateTime.Now},{record.ID}";
            await File.AppendAllTextAsync($"problematic_records{DateTime.Today}.csv", csvLine + Environment.NewLine);
            _logger.LogInformation("Информация о проблемной записи добавлена в файл: problematic_records.csv");
        }

    }

}

