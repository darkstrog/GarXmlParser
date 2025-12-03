using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ApartmentTypeDtoMapper : IDtoMapper<IMappedObject<ApartmentType>,ApartmentTypeDto>
    {
        private readonly ILogger _logger;
        public ApartmentTypeDtoMapper(ILogger<ApartmentTypeDtoMapper> logger)
        {
            _logger = logger;
        }
        public ApartmentTypeDto MapToDto(IMappedObject<ApartmentType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Обработка объекта: {ObjectId}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new ApartmentTypeDto
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
                _logger.LogError(ex, "Ошибка маппинга объекта: {ObjectId}", source.Entity.ID);
                throw;
            }
        }
    }
}
