using GarXmlParser;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers;
using System.Diagnostics;

namespace GarXmlParserConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string zipPath = "gar_xml.zip";

            string addressPattern = @"^AS_ADDR_OBJ_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string houseTypePattern = @"^AS_HOUSE_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string addressObjectDivisionPattern = @"^AS_ADDR_OBJ_DIVISION_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string addressObjectTypePattern = @"^AS_ADDR_OBJ_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string admHierarchyPattern = @"^AS_ADM_HIERARCHY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string apartmentTypePattern = @"^AS_APARTMENT_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string apartmentPattern = @"^AS_APARTMENTS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string carPlacePattern = @"^AS_CARPLACES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string changeHistoryPattern = @"^AS_CHANGE_HISTORY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string housesPattern = @"^AS_HOUSES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string munHierarchyPattern = @"^AS_MUN_HIERARCHY_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string normativeDocsPattern = @"^AS_NORMATIVE_DOCS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string normativeDocsKindsPattern = @"^AS_NORMATIVE_DOCS_KINDS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string normativeDocsTypesPattern = @"^AS_NORMATIVE_DOCS_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string objectLevelsPattern = @"^AS_OBJECT_LEVELS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string operationTypesPattern = @"^AS_OPERATION_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string paramsPattern = @"^AS_PARAM_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string AS_ROOMS_PARAMS_Pattern = @"^AS_ROOMS_PARAMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string paramTypesPattern = @"^AS_PARAM_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string reestrObjectsPattern = @"^AS_REESTR_OBJECTS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string roomTypesPattern = @"^AS_ROOM_TYPES_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string roomsPattern = @"^AS_ROOMS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";
            string steadsPattern = @"^AS_STEADS_\d{8}_[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}\.XML$";

            var garProcessor = new GarXmlProcessor();

            var houseTypeMapper = new HouseTypeNodeMapper();
            var addressObjectDivisionMapper = new AddressObjectDivisionNodeMapper();
            var addressObjectsMapper = new AddressObjectNodeMapper();
            var addressObjectsTypeMapper = new AddressObjectTypeNodeMapper();
            var admHierarchyMapper = new AdmHierarchyNodeMapper();
            var apartmentTypeMapper = new ApartmentTypeNodeMapper();
            var apartmentMapper = new ApartmentNodeMapper();
            var carPlaceMapper = new CarPlaceNodeMapper();
            var changeHistoryItemMapper = new ChangeHistoryItemNodeMapper();
            var housesMapper = new HousesNodeMapper();
            var munHierarchyNodeMapper = new MunHierarchyNodeMapper();
            var normativeDocsNodeMapper = new NormativeDocNodeMapper();
            var normativeDocKindNodeMapper = new NormativeDocKindNodeMapper();
            var normativeDocTypeNodeMapper = new NormativeDocTypeNodeMapper();
            var objectLevelNodeMapper = new ObjectLevelNodeMapper();
            var operationTypeNodeMapper = new OperationTypeNodeMapper();
            var paramNodeMapper = new ParamNodeMapper();
            var paramTypeNodeMapper = new ParamTypeNodeMapper();
            var reestrObjectNodeMapper = new ReestrObjectNodeMapper();
            var roomTypesNodeMapper = new RoomTypeNodeMapper();
            var roomNodeMapper = new RoomNodeMapper();
            var steadNodeMapper = new SteadNodeMapper();

            var time_processing = new Stopwatch();
            time_processing.Start();
            try
            {
                //Console.WriteLine($"House Types");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, houseTypePattern, houseTypeMapper)) 
                //{
                //    Console.WriteLine($"Получен HouseTypes: {item.NAME}");
                //}
                Console.WriteLine($"Address Objects");
                await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, addressPattern, addressObjectsMapper))
                {
                    Console.WriteLine($"Получен AddressObject: {item.NAME}");
                }
                //Console.WriteLine($"Address Objetcts Division");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, addressObjectDivisionPattern, addressObjectDivisionMapper))
                //{
                //    Console.WriteLine($"Получен AddressObjetctsDivision: {item.ID}");
                //}
                //Console.WriteLine($"Address Object Types");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, addressObjectTypePattern, addressObjectsTypeMapper))
                //{
                //    Console.WriteLine($"Получен AddressObjectType: {item.NAME}");
                //}
                //Console.WriteLine($"AdmHierarchyes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, admHierarchyPattern, admHierarchyMapper))
                //{
                //    Console.WriteLine($"Получен AdmHierarchy: {item.ID}");
                //}
                //Console.WriteLine($"ApartmentTypes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, apartmentTypePattern, apartmentTypeMapper))
                //{
                //    Console.WriteLine($"Получен ApartmentType: {item.NAME}");
                //}
                //Console.WriteLine($"Apartment");
                //var apartmentProgress = new Progress<ProcessingProgress>();
                //var status = new StatusLine();
                ////apartmentProgress.ProgressChanged += (sender, info) => { status.Update($"{info.TotalItemsProcessed}"); };//Console.WriteLine($"{info.TotalItemsProcessed}"); };
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, apartmentPattern, apartmentMapper,default, apartmentProgress))
                //{
                //    Console.WriteLine($"Получен Apartment: {item.ID}");
                //}
                //Console.WriteLine($"Carplaces");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, carPlacePattern, carPlaceMapper))
                //{
                //    Console.WriteLine($"Получен Carplace: {item.ID}");
                //}
                //Console.WriteLine($"ChangeHistory");
                //var changeHistoryProgress = new Progress<ProcessingProgress>();
                //changeHistoryProgress.ProgressChanged += (sender, info) => { Console.WriteLine($"{info.TotalItemsProcessed}"); };

                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, changeHistoryPattern, changeHistoryItemMapper,default, changeHistoryProgress))
                //{
                //    Console.WriteLine($"Получен ChangeHistoryItem: {item.OBJECTID}");   
                //}
                //Console.WriteLine($"Houses");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, housesPattern, housesMapper))
                //{
                //    Console.WriteLine($"Получен House: {item.ID}");
                //}
                //Console.WriteLine($"MunHierarchy");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, munHierarchyPattern, munHierarchyNodeMapper))
                //{
                //    Console.WriteLine($"Получен MunHierarchy: {item.ID}");
                //}
                //Console.WriteLine($"NormativeDocs");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, normativeDocsPattern, normativeDocsNodeMapper))
                //{
                //    Console.WriteLine($"Получен NormativeDoc: {item.NAME}");
                //}
                //Console.WriteLine($"NormativeDocsKinds");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, normativeDocsKindsPattern, normativeDocKindNodeMapper))
                //{
                //    Console.WriteLine($"Получен NormativeDocKind: {item.NAME}");
                //}
                //Console.WriteLine($"NormativeDocsTypes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, normativeDocsTypesPattern, normativeDocTypeNodeMapper))
                //{
                //    Console.WriteLine($"Получен NormativeDocType: {item.NAME}");
                //}
                //Console.WriteLine($"ObjectLevels");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, objectLevelsPattern, objectLevelNodeMapper))
                //{
                //    Console.WriteLine($"Получен ObjectLevel: {item.NAME}");
                //}
                //Console.WriteLine($"OperationTypes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, operationTypesPattern, operationTypeNodeMapper))
                //{
                //    Console.WriteLine($"Получен OperationType: {item.NAME}");
                //}
                //Console.WriteLine($"Params");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, AS_ROOMS_PARAMS_Pattern, paramNodeMapper))
                //{
                //    Console.WriteLine($"Получен Param: {item.VALUE}");
                //}
                //Console.WriteLine($"ParamTypes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, paramTypesPattern, paramTypeNodeMapper))
                //{
                //    Console.WriteLine($"Получен ParamType: {item.DESC}");
                //}
                //Console.WriteLine($"ReestrObjects");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, reestrObjectsPattern, reestrObjectNodeMapper))
                //{
                //    Console.WriteLine($"Получен ReestrObject: {item.OBJECTID}");
                //}
                //Console.WriteLine($"RoomTypes");
                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, roomTypesPattern, roomTypesNodeMapper))
                //{
                //    Console.WriteLine($"Получен RoomType: {item.ID}");
                //}
                Console.WriteLine($"Rooms");
                await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, roomsPattern, roomNodeMapper))
                {
                    Console.WriteLine($"Получен Room: {item.ID}");
                }
                //Console.WriteLine($"Steads");
                //var steadsProgress = new Progress<ProcessingProgress>();
                //var status = new StatusLine();

                //steadsProgress.ProgressChanged += (sender, info) => { Console.WriteLine($"Totalitems: {info.TotalItemsProcessed}"); };

                //await foreach (var item in garProcessor.StreamZipArchiveFilesAsync(zipPath, steadsPattern, steadNodeMapper, default, steadsProgress))
                //{
                //    status.Update($"Получен Stead: { item.ID}");
                //    //Console.WriteLine($"Получен Stead: {item.ID}");
                //}
                time_processing.Stop();
                var timeSpan = TimeSpan.FromMilliseconds(time_processing.ElapsedMilliseconds);
                Console.WriteLine($"метод закончил обработку за {timeSpan.Minutes}м : {timeSpan.Seconds}с : {timeSpan.Milliseconds}мс");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Возникла ошибка {ex.Message}");
            }
            
            Console.ReadLine();
        }
    }
}
