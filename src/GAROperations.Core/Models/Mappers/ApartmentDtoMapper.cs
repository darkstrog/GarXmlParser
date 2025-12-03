using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ApartmentDtoMapper : IDtoMapper<IMappedObject<Apartment>, ApartmentDto>
    {
        private readonly ILogger _logger;
        public ApartmentDtoMapper(ILogger<ApartmentDtoMapper> logger)
        {
            _logger = logger;
        }
        public ApartmentDto MapToDto(IMappedObject<Apartment> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг Apartment в DTO, ID: {ObjectId}}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new ApartmentDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    OBJECTGUID = entity.OBJECTGUID,
                    CHANGEID = entity.CHANGEID,
                    NUMBER = entity.NUMBER,
                    APARTTYPE = int.TryParse(entity.APARTTYPE, out int apartTypeId) ? apartTypeId : 0,
                    OPERTYPEID = entity.OPERTYPEID,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTUAL = entity.ISACTUAL == APARTMENTSAPARTMENTISACTUAL.Item1,
                    ISACTIVE = entity.ISACTIVE == APARTMENTSAPARTMENTISACTIVE.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга Apartment c ID: {ObjectId}", source.Entity.ID);
                throw;
            }
        }
    }
}
