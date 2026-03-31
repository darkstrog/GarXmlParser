using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Interfaces.Services;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;

namespace GAROperations.Infrastructure.Services
{
    public class ChangeHistoryService: IChangeHistoryService
    {
        private readonly ILogger<ChangeHistoryService> _logger;
        private readonly IChangeHistoryRepo _repository;
        private readonly IProblemDataService<ChangeHistoryItemDto> _problemDataService;

        public ChangeHistoryService(IChangeHistoryRepo repository, IProblemDataService<ChangeHistoryItemDto> problemDataService, ILogger<ChangeHistoryService> logger)
        {
            _logger = logger;
            _repository = repository;
            _problemDataService = problemDataService;
        }

        public async Task InsertDataAsync(ChangeHistoryItemDto data)
        {
            try
            {
                _logger?.LogInformation("Добавление одной записи CarPlace с ID: {ID} в базу", data.ADROBJECTID);
                await _repository.InsertAsync(data);
            }
            catch (Exception)
            {
                _logger?.LogError("Ошибка При добавлении записи CarPlace  с ID: {ID}", data.ADROBJECTID);
                throw;
            }
        }
        /// <summary>
        /// Метод для массовой асинхронной заливки объектов из асинхронного перечисляемого источника
        /// </summary>
        /// <param name="entities">IAsyncEnumerable коллекция Dto</param>
        /// <param name="batchSize">Размер пачки для оттравки одной транзакцией по умолчанию 1000</param>
        /// <returns></returns>
        public async Task InsertDataBulkAsync(IAsyncEnumerable<ChangeHistoryItemDto> entities, int batchSize = 1000)
        {
            var batch = new List<ChangeHistoryItemDto>(batchSize);

            await foreach (var item in entities)
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
        /// <param name="batch">Материализованная пачка объектов для отправки в бд</param>
        /// <param name="originalBatchSize">Оригинальный размер пачки объектов</param>
        /// <returns></returns>
        private async Task ProcessBatchWithFallback(List<ChangeHistoryItemDto> batch, int originalBatchSize)
        {
            try
            {
                await _repository.InsertBulkAsync(batch);
                _logger?.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Ошибка при обработке пачки из {Count} записей, дробим на более мелкие", batch.Count);

                await InsertEntitiesBulk(batch, Math.Max(1, originalBatchSize / 10));
            }
        }
        /// <summary>
        /// Метод для залики объектов в базу мелкими пачками
        /// Следующий метод в цепочке в случае неудачи будет пытаться записать объекты из пачки штучно,
        /// поэтому пачку не стоит делать большой
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public async Task InsertEntitiesBulk(IEnumerable<ChangeHistoryItemDto> entities, int batchSize = 100)
        {
            var batch = new List<ChangeHistoryItemDto>(batchSize);

            foreach (var item in entities)
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
        private async Task ProcessSingleBatchWithIsolation(List<ChangeHistoryItemDto> batch)
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
                        _logger.LogError(singleEx, "Не удалось записать в базу {RecordId} ,отправляем в сервис обработки проблемных данных", record.ADROBJECTID);
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
        private async Task HandleProblematicRecord(ChangeHistoryItemDto problematicRecord)
        {
            try
            {
                await _problemDataService.ProcessProblematicRecord(problematicRecord, null);
                _logger.LogInformation("Проблемная запись {RecordId} отправлена на анализ", problematicRecord.ADROBJECTID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить проблемную запись {RecordId} в сервис обработки", problematicRecord.ADROBJECTID);
                await WriteToCsvFallback(problematicRecord);
            }
        }
        /// <summary>
        /// Метод для логирования проблемных записей в случае недоступности сервиса обработки проблемных записей
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private async Task WriteToCsvFallback(ChangeHistoryItemDto record)
        {
            var csvLine = $"{DateTime.Now},{record.ADROBJECTID}";
            await File.AppendAllTextAsync($"problematic_records{DateTime.Today}.csv", csvLine + Environment.NewLine);
            _logger.LogInformation("Информация о проблемной записи добавлена в файл: problematic_records.csv");
        }
    }
}
