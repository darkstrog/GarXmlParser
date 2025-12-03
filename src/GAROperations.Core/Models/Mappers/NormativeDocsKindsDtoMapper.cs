using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class NormativeDocsKindsDtoMapper : IDtoMapper<IMappedObject<NormativeDocKind>,NormativeDocKindDto>
    {
        private readonly ILogger _logger;
        public NormativeDocsKindsDtoMapper(ILogger<NormativeDocsKindsDtoMapper> logger)
        {
            _logger = logger;
        }
        public NormativeDocKindDto MapToDto(IMappedObject<NormativeDocKind> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг NormativeDocKind в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new NormativeDocKindDto
                {
                    ID = int.Parse(entity.ID),
                    NAME = entity.NAME,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга NormativeDocKind с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
