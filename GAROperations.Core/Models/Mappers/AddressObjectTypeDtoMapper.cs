using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    internal class AddressObjectTypeDtoMapper : IDtoMapper<IMappedObject<AddressObjectType>, AddressObjectTypeDto>
    {
        private readonly ILogger _logger;

        public AddressObjectTypeDtoMapper(ILogger logger)
        {
            _logger = logger;
        }

        public AddressObjectTypeDto MapToDto(IMappedObject<AddressObjectType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Обработка объекта: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new AddressObjectTypeDto
                {
                    ID = int.Parse(entity.ID),
                    LEVEL = int.Parse(entity.LEVEL),
                    SHORTNAME = entity.SHORTNAME,
                    NAME = entity.NAME,
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
                _logger.LogError(ex, "Ошибка маппинга объекта: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
