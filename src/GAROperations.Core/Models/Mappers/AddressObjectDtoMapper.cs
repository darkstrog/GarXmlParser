using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class AddressObjectDtoMapper: IDtoMapper<IMappedObject<ADDRESSOBJECTSOBJECT>, AddressObjectDto>
    {
        private ILogger<AddressObjectDtoMapper> _logger;
        public AddressObjectDtoMapper(ILogger<AddressObjectDtoMapper> logger)
        {
            _logger = logger;
        }

        public AddressObjectDto MapToDto(IMappedObject<ADDRESSOBJECTSOBJECT> mappedObject)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Обработка объекта: {ObjectGuid}", mappedObject.Entity.OBJECTGUID);
                }
                var entity = mappedObject.Entity;
                return new AddressObjectDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    OBJECTGUID = Guid.TryParse(entity.OBJECTGUID, out Guid objectguid) ? objectguid : Guid.Empty,
                    CHANGEID = entity.CHANGEID,
                    NAME = entity.NAME ?? string.Empty,
                    TYPENAME = entity.TYPENAME ?? string.Empty,
                    LEVEL = entity.LEVEL ?? string.Empty,
                    OPERTYPEID = int.TryParse(entity.OPERTYPEID, out int operTypeId) ? operTypeId : 0,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTUAL = entity.ISACTUAL == ADDRESSOBJECTSOBJECTISACTUAL.Item1,
                    ISACTIVE = entity.ISACTIVE == ADDRESSOBJECTSOBJECTISACTIVE.Item1,
                    OriginalXMLString = mappedObject.OriginalXmlElement,
                    XmlFilePath = mappedObject.SourceFilePath
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга объекта: {ObjectGuid}", mappedObject.Entity.OBJECTGUID);
                throw;
            }
        }
    }
}
