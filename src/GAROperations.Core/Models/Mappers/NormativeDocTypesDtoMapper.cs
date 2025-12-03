using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAROperations.Core.Models.Mappers
{
    public class NormativeDocTypesDtoMapper : IDtoMapper<IMappedObject<NormativeDocType>, NormativeDocTypeDto>
    {
        private readonly ILogger _logger;
        public NormativeDocTypesDtoMapper(ILogger<NormativeDocTypesDtoMapper> logger)
        {
            _logger = logger;
        }
        public NormativeDocTypeDto MapToDto(IMappedObject<NormativeDocType> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг NormativeDocType в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new NormativeDocTypeDto
                {
                    ID = int.Parse(entity.ID),
                    NAME = entity.NAME,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга NormativeDocType с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
