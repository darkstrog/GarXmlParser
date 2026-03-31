using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ObjectLevelsDtoMapper : IDtoMapper<IMappedObject<ObjectLevel>,ObjectLevelDto>
    {
        private readonly ILogger _logger;
        public ObjectLevelsDtoMapper(ILogger<ObjectLevelsDtoMapper> logger)
        {
            _logger = logger;
        }
        public ObjectLevelDto MapToDto(IMappedObject<ObjectLevel> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг ObjectLevel в DTO, ID: {ID}", source.Entity.LEVEL);
                }
                var entity = source.Entity;
                return new ObjectLevelDto
                {
                    LEVEL = int.Parse(entity.LEVEL),
                    NAME = entity.NAME,
                    SHORTNAME = entity.SHORTNAME,
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
                _logger.LogError(ex, "Ошибка маппинга ObjectLevel с ID: {ID}", source.Entity.LEVEL);
                throw;
            }
        }
    }
}
