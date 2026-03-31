using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ReestrObjectsDtoMapper: IDtoMapper<IMappedObject<ReestrObject>,ReestrObjectDto>
    {
        private readonly ILogger _logger;
        public ReestrObjectsDtoMapper(ILogger<ReestrObjectsDtoMapper> logger)
        {
            _logger = logger;
        }
        public ReestrObjectDto MapToDto(IMappedObject<ReestrObject> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг ParamType в DTO, ID: {ID}", source.Entity.OBJECTID);
                }
                var entity = source.Entity;
                return new ReestrObjectDto
                {
                    OBJECTID = entity.OBJECTID,
                    CREATEDATE = entity.CREATEDATE,
                    CHANGEID = entity.CHANGEID,
                    LEVELID = int.Parse(entity.LEVELID),
                    UPDATEDATE = entity.UPDATEDATE,
                    OBJECTGUID = entity.OBJECTGUID,
                    ISACTIVE = entity.ISACTIVE == REESTR_OBJECTSOBJECTISACTIVE.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга ParamType с ID: {ID}", source.Entity.OBJECTID);
                throw;
            }
        }
    }
}
