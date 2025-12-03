using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class NormativeDocsDtoMapper :IDtoMapper<IMappedObject<NormativeDoc>,NormativeDocDto>
    {
        private readonly ILogger _logger;
        public NormativeDocsDtoMapper(ILogger<NormativeDocsDtoMapper> logger)
        {
            _logger = logger;
        }
        public NormativeDocDto MapToDto(IMappedObject<NormativeDoc> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг NormativeDoc в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new NormativeDocDto
                {
                    ID = entity.ID,
                    NAME = entity.NAME,
                    DATE = entity.DATE,
                    NUMBER = entity.NUMBER,
                    TYPE = int.Parse(entity.TYPE),
                    KIND = int.Parse(entity.KIND),
                    UPDATEDATE = entity.UPDATEDATE,
                    ORGNAME = entity.ORGNAME,
                    REGNUM = entity.REGNUM,
                    REGDATE = entity.REGDATE,
                    ACCDATE = entity.ACCDATE,
                    COMMENT = entity.COMMENT,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга NormativeDoc с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
