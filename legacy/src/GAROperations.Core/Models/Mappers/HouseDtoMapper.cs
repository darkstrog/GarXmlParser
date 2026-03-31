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
    public class HouseDtoMapper : IDtoMapper<IMappedObject<HOUSESHOUSE>, HouseDto>
    {
        private readonly ILogger _logger;
        public HouseDtoMapper(ILogger<HouseDtoMapper> logger)
        {
            _logger = logger;
        }
        public HouseDto MapToDto(IMappedObject<HOUSESHOUSE> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг House в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new HouseDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    OBJECTGUID = entity.OBJECTGUID,
                    CHANGEID = entity.CHANGEID,
                    HOUSENUM = entity.HOUSENUM,
                    ADDNUM1  = entity.ADDNUM1,
                    ADDNUM2 = entity.ADDNUM2,
                    HOUSETYPE = int.Parse(entity.HOUSETYPE),
                    ADDTYPE1 = int.Parse(entity.ADDTYPE1),
                    ADDTYPE2 = int.Parse(entity.ADDTYPE2),
                    OPERTYPEID = int.TryParse(entity.OPERTYPEID, out int operTypeId) ? operTypeId : 0,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    STARTDATE = entity.STARTDATE,
                    UPDATEDATE = entity.UPDATEDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTUAL = entity.ISACTUAL == HOUSESHOUSEISACTUAL.Item1,
                    ISACTIVE = entity.ISACTIVE == HOUSESHOUSEISACTIVE.Item1,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга House с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
