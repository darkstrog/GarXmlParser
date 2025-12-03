using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class RoomsDtoMapper:IDtoMapper<IMappedObject<Room>,RoomDto>
    {
        private readonly ILogger _logger;
        public RoomsDtoMapper(ILogger<RoomsDtoMapper> logger)
        {
            _logger = logger;
        }
        public RoomDto MapToDto(IMappedObject<Room> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг Room в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new RoomDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    OBJECTGUID = entity.OBJECTGUID,
                    CHANGEID = entity.CHANGEID,
                    NUMBER = entity.NUMBER,
                    ROOMTYPE = int.Parse(entity.ROOMTYPE),
                    OPERTYPEID = int.Parse(entity.OPERTYPEID),
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTUAL = entity.ISACTUAL == ROOMSROOMISACTUAL.Item1,
                    ISACTIVE = entity.ISACTIVE == ROOMSROOMISACTIVE.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга Room с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
