using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class HouseTypeDtoMapper : IDtoMapper<IMappedObject<HOUSETYPESHOUSETYPE>, HouseTypeDto>
    {
        private readonly ILogger _logger;
        public HouseTypeDtoMapper(ILogger<HouseDtoMapper> logger)
        {
            _logger = logger;
        }
        public HouseTypeDto MapToDto(IMappedObject<HOUSETYPESHOUSETYPE> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг HouseType в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new HouseTypeDto
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
                _logger.LogError(ex, "Ошибка маппинга HouseType с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
