using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class AddressObjectDivisionDtoMapper : IDtoMapper<IMappedObject<AddressObjectDivisionItem>, AddressObjectDivisionItemDto>
    {
        private readonly ILogger _logger;
        public AddressObjectDivisionDtoMapper(ILogger<AddressObjectDivisionDtoMapper> logger)
        {
            _logger = logger;
        }
        public AddressObjectDivisionItemDto MapToDto(IMappedObject<AddressObjectDivisionItem> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Обработка объекта: {ObjectId}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new AddressObjectDivisionItemDto
                {
                    ID = entity.ID,
                    CHANGEID = entity.CHANGEID,
                    PARENTID = entity.PARENTID,
                    CHILDID = entity.CHILDID,
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
