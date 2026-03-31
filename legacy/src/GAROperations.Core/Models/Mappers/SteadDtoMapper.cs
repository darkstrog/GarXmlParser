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
    public class SteadDtoMapper: IDtoMapper<IMappedObject<Stead>,SteadDto>
    {
        private readonly ILogger _logger;
        public SteadDtoMapper(ILogger<SteadDtoMapper> logger)
        {
            _logger = logger;
        }
        public SteadDto MapToDto(IMappedObject<Stead> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг Stead в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new SteadDto
                {
                    ID = int.Parse(entity.ID),
                    OBJECTID = int.Parse(entity.OBJECTID),
                    OBJECTGUID = entity.OBJECTGUID,
                    CHANGEID = int.Parse(entity.CHANGEID),
                    NUMBER = entity.NUMBER,
                    OPERTYPEID = entity.OPERTYPEID,
                    PREVID = int.Parse(entity.PREVID),
                    NEXTID = int.Parse(entity.NEXTID),
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTIVE = entity.ISACTIVE == STEADSSTEADISACTIVE.Item1,
                    ISACTUAL = entity.ISACTUAL == STEADSSTEADISACTUAL.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга Stead с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
