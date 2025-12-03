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
        /// <summary>
        /// Заливка в базу AddressObjectDivision
        /// </summary>
        /// <param name="garProcessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        /// <param name="token"></param>
        /// <param name="zipPath"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Заливка в базу AddressObject
        /// </summary>
        /// <param name="garProcessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        /// <param name="token"></param>
        /// <param name="zipPath"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Заливка в базу AdmHierarchy
        /// </summary>
        /// <param name="garProcessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        /// <param name="token"></param>
        /// <param name="zipPath"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Заливка в базу AddressObjectType
        /// </summary>
        /// <param name="garProcessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        /// <param name="token"></param>
        /// <param name="zipPath"></param>
        /// <returns></returns>
        public async Task<bool> AddressObjectTypeDataProcess(GarXmlProcessor garProcessor, ILoggerFactory loggerFactory,
                                                    string connectionString, CancellationToken token,
                                                    string zipPath)
        {
            #region Loggers

            ILogger<JsonProblemDataService<AddressObjectTypeDto>> problemDataServiceLog = loggerFactory.CreateLogger<JsonProblemDataService<AddressObjectTypeDto>>();
            ILogger<AddressObjectTypeService> serviceLog = loggerFactory.CreateLogger<AddressObjectTypeService>();
            ILogger<AddressObjectTypeDtoMapper> dtoMapperLog = loggerFactory.CreateLogger<AddressObjectTypeDtoMapper>();
            ILogger<AddressObjectTypeRepo> repoLog = loggerFactory.CreateLogger<AddressObjectTypeRepo>();

            #endregion

            #region regex_pattern

            string Pattern = @"^AS_ADDR_OBJ_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

            #endregion

            #region mappers

            AddressObjectTypeNodeMapper dataMapper = new AddressObjectTypeNodeMapper();
            AddressObjectTypeDtoMapper dtoMapper = new AddressObjectTypeDtoMapper(dtoMapperLog);

            #endregion
            //Настройка сообщений прогресса парсинга
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

            JsonProblemDataService<AddressObjectTypeDto> jsonProblemDataService = new JsonProblemDataService<AddressObjectTypeDto>(problemDataServiceLog);

            AddressObjectTypeService entityService = new AddressObjectTypeService(new AddressObjectTypeRepo(connectionString, repoLog), jsonProblemDataService, serviceLog);

            var data = garProcessor.StreamZipArchiveFilesAsync(zipPath, Pattern, dataMapper, token, progress);
            await entityService.InsertDataBulkAsync(dtoMapper.MapToDtoAsync(data, token));
            return true;
        }

        /// <summary>
        /// Заливка в базу AddressObjectParams
        /// </summary>
        /// <param name="garProcessor"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        /// <param name="token"></param>
        /// <param name="zipPath"></param>
        /// <param name="regions"></param>
        /// <returns></returns>
        public async Task<bool> AddressObjectParamsDataProcess(GarXmlProcessor garProcessor, ILoggerFactory loggerFactory,
                                                    string connectionString, CancellationToken token,
                                                    string zipPath, IEnumerable<int>? regions = null)
        {
            #region Loggers

            ILogger<JsonProblemDataService<ParamDto>> problemDataServiceLog = loggerFactory.CreateLogger<JsonProblemDataService<ParamDto>>();
            ILogger<AddressObjectParamsService> serviceLog = loggerFactory.CreateLogger<AddressObjectParamsService>();
            //Схема Param общая для всех сущностей содержищих в названии Param поэтому и класс DTO для них общий.
            ILogger<ParamDtoMapper> dtoMapperLog = loggerFactory.CreateLogger<ParamDtoMapper>();
            ILogger<AddressObjectParamsRepo> repoLog = loggerFactory.CreateLogger<AddressObjectParamsRepo>();

            #endregion

            #region regex_pattern

            var regionsFilter = "0[1-9]|[1-9][0-9]";
            if (regions != null)
            {
                regionsFilter = string.Join("|", regions.Select(n => n.ToString("D2")));
            }

            string Pattern = @$"^(?:{regionsFilter})[/\\]" + @"AS_ADDR_OBJ_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

            #endregion

            #region mappers

            ParamNodeMapper dataMapper = new ParamNodeMapper();
            ParamDtoMapper dtoMapper = new ParamDtoMapper(dtoMapperLog);

            #endregion
            //Настройка сообщений прогресса парсинга
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

            JsonProblemDataService<ParamDto> jsonProblemDataService = new JsonProblemDataService<ParamDto>(problemDataServiceLog);

            AddressObjectParamsService entityService = new AddressObjectParamsService(new AddressObjectParamsRepo(connectionString, repoLog), jsonProblemDataService, serviceLog);

            var data = garProcessor.StreamZipArchiveFilesAsync(zipPath, Pattern, dataMapper, token, progress);
            await entityService.InsertDataBulkAsync(dtoMapper.MapToDtoAsync(data, token));
            return true;
        }
    }
}