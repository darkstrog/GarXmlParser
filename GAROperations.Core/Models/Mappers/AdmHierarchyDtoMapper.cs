using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class AdmHierarchyDtoMapper : IDtoMapper<IMappedObject<AdmHierarchy>, AdmHierarchyDto>
    {
        private ILogger<AdmHierarchyDtoMapper> _logger;

        public AdmHierarchyDtoMapper(ILogger<AdmHierarchyDtoMapper> logger)
        {
            _logger = logger;
        }

        public AdmHierarchyDto MapToDto(IMappedObject<AdmHierarchy> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Обработка объекта: {ID}", source.Entity.ID);
                }
                var entity = source.Entity;
                return new AdmHierarchyDto
                {
                    ID = entity.ID,
                    OBJECTID = entity.OBJECTID,
                    PARENTOBJID = entity.PARENTOBJIDSpecified ? entity.PARENTOBJID : null,
                    CHANGEID = entity.CHANGEID,
                    REGIONCODE = entity.REGIONCODE ?? string.Empty,
                    AREACODE = entity.AREACODE ?? string.Empty,
                    CITYCODE = entity.CITYCODE ?? string.Empty,
                    PLACECODE = entity.PLACECODE ?? string.Empty,
                    PLANCODE = entity.PLANCODE ?? string.Empty,
                    STREETCODE = entity.STREETCODE ?? string.Empty,
                    PREVID = entity.PREVIDSpecified ? entity.PREVID : null,
                    NEXTID = entity.NEXTIDSpecified ? entity.NEXTID : null,
                    UPDATEDATE = entity.UPDATEDATE,
                    STARTDATE = entity.STARTDATE,
                    ENDDATE = entity.ENDDATE,
                    ISACTIVE = entity.ISACTIVE == AdmHierarchyITEMISACTIVE.Item1,
                    PATH = entity.PATH ?? string.Empty,
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
