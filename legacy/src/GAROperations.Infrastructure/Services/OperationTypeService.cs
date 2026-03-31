using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Interfaces.Services;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;

namespace GAROperations.Infrastructure.Services
{
    public class OperationTypeService : IOperationTypeService
    {
        private readonly ILogger<OperationTypeService> _logger;
        private readonly IOperationTypeRepo _repository;
        private readonly IProblemDataService<OperationTypeDto> _problemDataService;

        public OperationTypeService(IOperationTypeRepo repository, IProblemDataService<OperationTypeDto> problemDataService, ILogger<OperationTypeService> logger)
        {
            _logger = logger;
            _repository = repository;
            _problemDataService = problemDataService;
        }
        public async Task InsertDataAsync(OperationTypeDto data)
        {
            try
            {
                _logger?.LogInformation("Добавление одной записи OperationType с ID: {level} в базу", data.ID);
                await _repository.InsertAsync(data);
            }
            catch (Exception)
            {
                _logger?.LogError("Ошибка При добавлении записи OperationType с ID: {level}", data.ID);
                throw;
            }
        }
        /// <summary>
        /// Метод для массовой асинхронной заливки объектов из асинхронного перечисляемого источника
        /// </summary>
        /// <param name="entities">IAsyncEnumerable коллекция Dto</param>
        /// <param name="batchSize">Размер пачки для оттравки одной транзакцией по умолчанию 1000</param>
        /// <returns></returns>
        public async Task InsertDataBulkAsync(IAsyncEnumerable<OperationTypeDto> entities, int batchSize = 1000)
        {
            var batch = new List<OperationTypeDto>(batchSize);

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
        private async Task ProcessBatchWithFallback(List<OperationTypeDto> batch, int originalBatchSize)
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
        public async Task InsertEntitiesBulk(IEnumerable<OperationTypeDto> entities, int batchSize = 100)
        {
            var batch = new List<OperationTypeDto>(batchSize);

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
        private async Task ProcessSingleBatchWithIsolation(List<OperationTypeDto> batch)
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
                        _logger.LogError(singleEx, "Не удалось записать в базу {ID} ,отправляем в сервис обработки проблемных данных", record.ID);
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
        private async Task HandleProblematicRecord(OperationTypeDto problematicRecord)
        {
            try
            {
                await _problemDataService.ProcessProblematicRecord(problematicRecord, null);
                _logger.LogInformation("Проблемная запись {ID} отправлена на анализ", problematicRecord.ID);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить проблемную запись {ID} в сервис обработки", problematicRecord.ID);
                await WriteToCsvFallback(problematicRecord);
            }
        }
        /// <summary>
        /// Метод для логирования проблемных записей в случае недоступности сервиса обработки проблемных записей
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private async Task WriteToCsvFallback(OperationTypeDto record)
        {
            var csvLine = $"{DateTime.Now},{record.ID}";
            await File.AppendAllTextAsync($"problematic_records{DateTime.Today}.csv", csvLine + Environment.NewLine);
            _logger.LogInformation("Информация о проблемной записи добавлена в файл: problematic_records.csv");
        }
    }
}
