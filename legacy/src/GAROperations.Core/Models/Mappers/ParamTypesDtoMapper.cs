using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ParamTypesDtoMapper: IDtoMapper<IMappedObject<ParamType>,ParamTypeDto>
    {
        private readonly ILogger _logger;
        public ParamTypesDtoMapper(ILogger<ParamTypesDtoMapper> logger)
        {
            _logger = logger;
        }
        public ParamTypeDto MapToDto(IMappedObject<ParamType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг ParamType в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new ParamTypeDto
                {
                    ID = int.Parse(entity.ID),
                    NAME = entity.NAME,
                    CODE = entity.CODE,
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
                _logger.LogError(ex, "Ошибка маппинга ParamType с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
