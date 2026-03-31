using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class MunHierarchyDtoMapper : IDtoMapper<IMappedObject<MunHierarchy>,MunHierarchyDto>
    {
        private readonly ILogger _logger;
        public MunHierarchyDtoMapper(ILogger<MunHierarchyDtoMapper> logger)
        {
            _logger = logger;
        }
        public MunHierarchyDto MapToDto(IMappedObject<MunHierarchy> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг MunHierarchy в DTO, ID: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new MunHierarchyDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    PARENTOBJID = entity.PARENTOBJID,
                    CHANGEID = entity.CHANGEID,
                    OKTMO = entity.OKTMO,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    UPDATEDATE = entity.UPDATEDATE,
                    ISACTIVE = entity.ISACTIVE == ITEMSITEMISACTIVE.Item1,
                    PATH = entity.PATH,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга MunHierarchy с ID: {ID}", source.Entity.ID);
                throw;
            }
        }
    }
}
