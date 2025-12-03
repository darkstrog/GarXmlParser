using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ParamDtoMapper: IDtoMapper<IMappedObject<Param>,ParamDto>
    {
        private readonly ILogger _logger;
        public ParamDtoMapper(ILogger<ParamDtoMapper> logger)
        {
            _logger = logger;
        }
        public ParamDto MapToDto(IMappedObject<Param> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг Param в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new ParamDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    CHANGEID = entity.CHANGEID,
                    CHANGEIDEND = entity.CHANGEIDEND,
                    TYPEID = int.Parse(entity.TYPEID),
                    VALUE = entity.VALUE,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга Param с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
