using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class RoomTypesDtoMapper: IDtoMapper<IMappedObject<RoomType>,RoomTypeDto>
    {
        private readonly ILogger _logger;
        public RoomTypesDtoMapper(ILogger<RoomTypesDtoMapper> logger)
        {
            _logger = logger;
        }
        public RoomTypeDto MapToDto(IMappedObject<RoomType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг RoomType в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new RoomTypeDto
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
                _logger.LogError(ex, "Ошибка маппинга RoomType с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
