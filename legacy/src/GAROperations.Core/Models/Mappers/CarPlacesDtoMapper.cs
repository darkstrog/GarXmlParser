using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class CarPlacesDtoMapper : IDtoMapper<IMappedObject<CarPlace>, CarPlaceDto>
    {
        private readonly ILogger _logger;
        public CarPlacesDtoMapper(ILogger<CarPlacesDtoMapper> logger)
        {
            _logger = logger;
        }
        public CarPlaceDto MapToDto(IMappedObject<CarPlace> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг CarPlace в DTO, ID: {ObjectId}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new CarPlaceDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    OBJECTGUID = entity.OBJECTGUID,
                    CHANGEID = entity.CHANGEID,
                    NUMBER = entity.NUMBER,
                    OPERTYPEID = int.TryParse(entity.OPERTYPEID, out int operTypeId) ? operTypeId : 0,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTIVE = entity.ISACTIVE == CARPLACESCARPLACEISACTIVE.Item1,
                    ISACTUAL = entity.ISACTUAL == CARPLACESCARPLACEISACTUAL.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга CarPlace с ID: {ObjectId}", source.Entity.ID);
                throw;
            }
        }
    }
}
