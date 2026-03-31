using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class OperationTypesDtoMapper : IDtoMapper<IMappedObject<OperationType>,OperationTypeDto>
    {
        private readonly ILogger _logger;
        public OperationTypesDtoMapper(ILogger<OperationTypesDtoMapper> logger)
        {
            _logger = logger;
        }
        public OperationTypeDto MapToDto(IMappedObject<OperationType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг OperationType в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new OperationTypeDto
                {
                    ID = int.Parse(entity.ID),
                    NAME = entity.NAME,
                    SHORTNAME = entity.SHORTNAME,
                    DESC = entity.DESC,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTIVE = entity.ISACTIVE,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга OperationType с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
