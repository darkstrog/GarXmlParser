using GARReplication.Core.EntityWriters;
using GARReplication.Core.Interfaces;
using GarXmlParser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace GARReplication.Core.Services
{
    public class ReplicationService: IReplicationService
    {
        private readonly ILogger<ReplicationService> _logger;
        private readonly IBulkRepository _bulkRepository;
        private readonly IGarXmlProcessor _xmlProcessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, IWriterSelectionStrategy> _strategies;

        public ReplicationService(ILogger<ReplicationService> logger, 
                                    IGarXmlProcessor xmlProcessor,
                                    IBulkRepository bulkRepository,
                                    IServiceProvider serviceProvider,
                                    IEnumerable<IWriterSelectionStrategy> strategies)
        {
            _logger = logger;
            _bulkRepository = bulkRepository;
            _xmlProcessor = xmlProcessor;
            _serviceProvider = serviceProvider;
            _strategies = strategies.ToDictionary(
            s => s.GetType().Name.Replace("Strategy", "").ToLower(),
            s => s);
        }

        public async Task ReplicateAsync(string zipPath, 
                                         int batchSize,
                                         IEnumerable<int>? regions = null, 
                                         string strategyName = "FullRepl",
                                         CancellationToken token = default)
        {
            if (!_strategies.TryGetValue(strategyName.ToLower(), out var strategy))
                throw new ArgumentException($"Неизвестная стратегия репликации: {strategyName}");

            await ReplicateAsync(zipPath, strategy, batchSize, regions, token);
        }

        public async Task ReplicateAsync(string zipPath, 
                                         IWriterSelectionStrategy strategy, 
                                         int batchSize, 
                                         IEnumerable<int>? regions = null, 
                                         CancellationToken token= default)
        {
            var writerTypes = strategy.GetWriterTypes();
            var regionsFilter = "0[1-9]|[1-9][0-9]";
            int totalObjects= 0;

            if (regions != null && regions.Any())
            {
                regionsFilter = string.Join("|", regions.Select(n => n.ToString("D2")));
            }

            var batch = new Queue<XElement>();

            foreach (var writerType in writerTypes)
            {
                var writer = (IEntityWriter)_serviceProvider.GetRequiredService(writerType);
                _logger?.LogDebug("Начинается работа {entity}", writerType.Name);
                string Pattern = @$"^(?:{regionsFilter})[/\\]{writer.FilePattern}";
                var data = _xmlProcessor.StreamZipArchiveFilesAsync(zipPath, Pattern , writer.NodeName, token, GetProgress());
                await foreach (var item in data)
                {
                    batch.Enqueue(item);

                    if (batch.Count >= batchSize)
                    {
                        var _count = batch.Count;
                        await _bulkRepository.InsertBulkAsync(batch, writer);
                        totalObjects += _count;
                        _logger?.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);

                    }
                }
                if (batch.Count > 0)
                {
                    var _count = batch.Count;
                    await _bulkRepository.InsertBulkAsync(batch, writer);
                    totalObjects += _count;
                    _logger?.LogDebug("Успешно обработана пачка из {Count} записей", batch.Count);
                }

            }
            _logger?.LogInformation("Всего в базу записано - [ {objects} ]", totalObjects);

        }

        private Progress<ProcessingProgress> GetProgress()
        {
            var progress = new Progress<ProcessingProgress>(report =>
            {
                var message = DateTime.Now + $" Прогресс маппинга XML: Всего файлов: {report.TotalFiles} " +
                              $"| Обработано файлов: {report.CurrentFileIndex} " +
                              $"| Текущий файл:{Path.GetFileName(report.CurrentFilePath)} " +
                              $"| Получено объектов: {report.TotalItemsProcessed} " +
                              $"| Ошибок: {report.failedItems}";
                Console.WriteLine(message);
            });
            return progress;
        }
    }

    public interface IWriterSelectionStrategy
    {
        IEnumerable<Type> GetWriterTypes();
    }
    //Полная репликация сущностей ГАР
    public class FullReplStrategy : IWriterSelectionStrategy
    {
        public IEnumerable<Type> GetWriterTypes() => new[]
        {
            typeof(AddressObjectDivisionWriter),
            typeof(AddressObjectWriter),
            typeof(AddressObjectTypeWriter),
            typeof(AdmHierarchyWriter),
            typeof(ApartmentTypeWriter),
            typeof(ApartmentWriter),
            typeof(CarPlaceWriter),
            typeof(ChangeHistoryWriter),
            typeof(HouseTypeWriter),
            typeof(HouseWriter),
            typeof(MunHierarchyWriter),
            typeof(NormativeDocKindWriter),
            typeof(NormativeDocTypeWriter),
            typeof(NormativeDocWriter),
            typeof(ObjectLevelWriter),
            typeof(OperationTypeWriter),
            typeof(ParamTypeWriter),
            typeof(ReestrObjectWriter),
            typeof(RoomTypeWriter),
            typeof(RoomWriter),
            typeof(SteadWriter),
            typeof(AddressObjectParamsWriter),
            typeof(ApartmentsParamsWriter),
            typeof(CarPlaceParamsWriter),
            typeof(HousesParamsWriter),
            typeof(RoomsParamsWriter),
            typeof(SteadsParamsWriter)
        };
    }
    
    //Полный набор сущностей без истории изменений
    public class WithoutCHReplStrategy : IWriterSelectionStrategy
    {
        public IEnumerable<Type> GetWriterTypes() => new[]
        {
            typeof(AddressObjectDivisionWriter),
            typeof(AddressObjectWriter),
            typeof(AddressObjectTypeWriter),
            typeof(AdmHierarchyWriter),
            typeof(ApartmentTypeWriter),
            typeof(ApartmentWriter),
            typeof(CarPlaceWriter),
            typeof(HouseTypeWriter),
            typeof(HouseWriter),
            typeof(MunHierarchyWriter),
            typeof(NormativeDocKindWriter),
            typeof(NormativeDocTypeWriter),
            typeof(NormativeDocWriter),
            typeof(ObjectLevelWriter),
            typeof(OperationTypeWriter),
            typeof(ParamTypeWriter),
            typeof(ReestrObjectWriter),
            typeof(RoomTypeWriter),
            typeof(RoomWriter),
            typeof(SteadWriter),
            typeof(AddressObjectParamsWriter),
            typeof(ApartmentsParamsWriter),
            typeof(CarPlaceParamsWriter),
            typeof(HousesParamsWriter),
            typeof(RoomsParamsWriter),
            typeof(SteadsParamsWriter)
        };
    }
    public class SearchServiceStrategy : IWriterSelectionStrategy
    {
        public IEnumerable<Type> GetWriterTypes() => new[]
        {
            typeof(AddressObjectWriter),
            typeof(AdmHierarchyWriter),
            typeof(ApartmentWriter),
            typeof(HouseWriter),
        };
    }
}
