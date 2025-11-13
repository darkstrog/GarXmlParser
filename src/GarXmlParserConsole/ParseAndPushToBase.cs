using GAROperations.Core.Models.GarModels;
using GAROperations.Core.Models.Mappers;
using GAROperations.Infrastructure.Data.Repository.GarRepo;
using GAROperations.Infrastructure.Services;
using GarXmlParser;
using GarXmlParser.Mappers;
using Microsoft.Extensions.Logging;

namespace GarXmlParserConsole
{
    internal class ParseAndPushToBase
    {
        public async Task<bool> AddressObjectDivisionDataProcess(GarXmlProcessor garProcessor, ILoggerFactory loggerFactory,
                                                    string connectionString, CancellationToken token,
                                                    string zipPath, IEnumerable<int>? regions = null)
        {
            ILogger<JsonProblemDataService<AddressObjectDivisionItemDto>> problemDataServiceLog = loggerFactory.CreateLogger<JsonProblemDataService<AddressObjectDivisionItemDto>>();
            ILogger<AddressObjectDivisionItemService> AddressObjectDivisionItemLog = loggerFactory.CreateLogger<AddressObjectDivisionItemService>();
            ILogger<AddressObjectDivisionDtoMapper> AddressObjectDivisionDtoMapperLog = loggerFactory.CreateLogger<AddressObjectDivisionDtoMapper>();
            ILogger<AddressObjectDivisionItemRepo> AddressObjectDivisionItemrepoLog = loggerFactory.CreateLogger<AddressObjectDivisionItemRepo>();
            var regionsFilter = "0[1-9]|[1-9][0-9]";
            if (regions != null)
            {
                regionsFilter = string.Join("|", regions.Select(n => n.ToString("D2")));
            }

            string addressObjectDivisionPattern = @$"^(?:{regionsFilter})[/\\]" + @"AS_ADDR_OBJ_DIVISION_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";


            AddressObjectDivisionNodeMapper addressObjectDivisionMapper = new AddressObjectDivisionNodeMapper();
            AddressObjectDivisionDtoMapper addressObjectDivisionItemDtoMapper = new AddressObjectDivisionDtoMapper(AddressObjectDivisionDtoMapperLog);

            var progress = new Progress<ProcessingProgress>(report =>
            {
                var message = DateTime.Now + $" Прогресс маппинга XML: Всего файлов: {report.TotalFiles} " +
                              $"| Обработано файлов: {report.CurrentFileIndex} " +
                              $"| Текущий файл:{Path.GetFileName(report.CurrentFilePath)} " +
                              $"| Получено объектов: {report.TotalItemsProcessed} " +
                              $"| Ошибок: {report.failedItems}";
                Console.WriteLine(message);
                Console.ResetColor();
            });

            JsonProblemDataService<AddressObjectDivisionItemDto> jsonProblemDataService = new JsonProblemDataService<AddressObjectDivisionItemDto>(problemDataServiceLog);

            AddressObjectDivisionItemService addressObjectDivisionItemService = new AddressObjectDivisionItemService(new AddressObjectDivisionItemRepo(connectionString, AddressObjectDivisionItemrepoLog), jsonProblemDataService, AddressObjectDivisionItemLog);

            var data = garProcessor.StreamZipArchiveFilesAsync(zipPath, addressObjectDivisionPattern, addressObjectDivisionMapper, token, progress);
            await addressObjectDivisionItemService.InsertDataBulkAsync(addressObjectDivisionItemDtoMapper.MapToDtoAsync(data, token));
            return true;
        }

        public async Task<bool> AddressObjectDataProcess(GarXmlProcessor garProcessor, ILoggerFactory loggerFactory,
                                                        string connectionString, CancellationToken token,
                                                        string zipPath, IEnumerable<int>? regions = null)
        {
            var progress = new Progress<ProcessingProgress>(report =>
            {
                var message = DateTime.Now + $"Прогресс маппинга XML: Всего файлов: {report.TotalFiles} " +
                              $"| Обработано файлов: {report.CurrentFileIndex} " +
                              $"| Текущий файл:{Path.GetFileName(report.CurrentFilePath)} " +
                              $"| Получено объектов: {report.TotalItemsProcessed} " +
                              $"| Ошибок: {report.failedItems}";
                Console.WriteLine(message);
            });

            var regionsFilter = "0[1-9]|[1-9][0-9]";
            if (regions != null)
            {
                regionsFilter = string.Join("|", regions.Select(n => n.ToString("D2")));
            }

            string addressPattern = @$"^(?:{regionsFilter})[/\\]" + @"AS_ADDR_OBJ_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            AddressObjectNodeMapper addressObjectsMapper = new AddressObjectNodeMapper();
            ILogger<JsonProblemDataService<AddressObjectDto>> problemDataServiceLog = loggerFactory.CreateLogger<JsonProblemDataService<AddressObjectDto>>();
            ILogger<AddressObjectService> AddressObjectLog = loggerFactory.CreateLogger<AddressObjectService>();
            ILogger<AddressObjectDtoMapper> addressObjectDtoMapperLog = loggerFactory.CreateLogger<AddressObjectDtoMapper>();
            ILogger<AddressObjectRepo> addressObjectrepoLog = loggerFactory.CreateLogger<AddressObjectRepo>();

            JsonProblemDataService<AddressObjectDto> jsonProblemDataService = new JsonProblemDataService<AddressObjectDto>(problemDataServiceLog);


            AddressObjectService addressObjectService = new AddressObjectService(new AddressObjectRepo(connectionString, addressObjectrepoLog), jsonProblemDataService, AddressObjectLog);

            var data = garProcessor.StreamZipArchiveFilesAsync(zipPath, addressPattern, addressObjectsMapper, token, progress);

            AddressObjectDtoMapper addressObjectDtoMapper = new AddressObjectDtoMapper(addressObjectDtoMapperLog);

            await addressObjectService.InsertDataBulkAsync(addressObjectDtoMapper.MapToDtoAsync(data, token));
            return true;
        }

        public async Task<bool> AdmHierarchyDataProcess(GarXmlProcessor garProcessor, ILoggerFactory loggerFactory,
                                                        string connectionString, CancellationToken token,
                                                        string zipPath, IEnumerable<int>? regions = null)
        {
            var progress = new Progress<ProcessingProgress>(report =>
            {
                var message = DateTime.Now + $"Прогресс маппинга XML: Всего файлов: {report.TotalFiles} " +
                              $"| Обработано файлов: {report.CurrentFileIndex} " +
                              $"| Текущий файл:{Path.GetFileName(report.CurrentFilePath)} " +
                              $"| Получено объектов: {report.TotalItemsProcessed} " +
                              $"| Ошибок: {report.failedItems}";
                Console.WriteLine(message);
            });

            var regionsFilter = "0[1-9]|[1-9][0-9]";
            if (regions != null)
            {
                regionsFilter = string.Join("|", regions.Select(n => n.ToString("D2")));
            }

            string admHierarchyPattern = @$"^(?:{regionsFilter})[/\\]" + @"AS_ADM_HIERARCHY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            AdmHierarchyNodeMapper admHierarchyMapper = new AdmHierarchyNodeMapper();
            ILogger<JsonProblemDataService<AdmHierarchyDto>> problemDataServiceLog = loggerFactory.CreateLogger<JsonProblemDataService<AdmHierarchyDto>>();
            ILogger<AdmHierarchyService> admHierarchyLog = loggerFactory.CreateLogger<AdmHierarchyService>();
            ILogger<AdmHierarchyDtoMapper> admHierarchyDtoMapperLog = loggerFactory.CreateLogger<AdmHierarchyDtoMapper>();
            ILogger<AdmHierarchyRepo> admHierarchyrepoLog = loggerFactory.CreateLogger<AdmHierarchyRepo>();

            JsonProblemDataService<AdmHierarchyDto> jsonProblemDataService = new JsonProblemDataService<AdmHierarchyDto>(problemDataServiceLog);


            AdmHierarchyService admHierarchyService = new AdmHierarchyService(new AdmHierarchyRepo(connectionString, admHierarchyrepoLog), jsonProblemDataService, admHierarchyLog);

            var data = garProcessor.StreamZipArchiveFilesAsync(zipPath, admHierarchyPattern, admHierarchyMapper, token, progress);

            AdmHierarchyDtoMapper admHierarchyDtoMapper = new AdmHierarchyDtoMapper(admHierarchyDtoMapperLog);

            await admHierarchyService.InsertDataBulkAsync(admHierarchyDtoMapper.MapToDtoAsync(data, token));
            return true;
        }
    }
}