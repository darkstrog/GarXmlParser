using GAROperations.Core.Interfaces.Mappers;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;

namespace GAROperations.Core.Models.Mappers
{
    public class ChangeHistoryItemDtoMapper : IDtoMapper<IMappedObject<ChangeHistoryItem>, ChangeHistoryItemDto>
    {
        private readonly ILogger _logger;
        public ChangeHistoryItemDtoMapper(ILogger<ChangeHistoryItemDtoMapper> logger)
        {
            _logger = logger;
        }
        public ChangeHistoryItemDto MapToDto(IMappedObject<ChangeHistoryItem> source)
        {
            try
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug("Маппинг ChangeHistoryItem в DTO, CHANGEID: {CHANGEID} - OBJECTID: {OBJECTID}", source.Entity.CHANGEID, source.Entity.OBJECTID);
                }
                var entity = source.Entity;
                return new ChangeHistoryItemDto
                {
                    CHANGEID = entity.CHANGEID,
                    OBJECTID = entity.OBJECTID,
                    ADROBJECTID = entity.ADROBJECTID,
                    OPERTYPEID = int.TryParse(entity.OPERTYPEID, out int operTypeId) ? operTypeId : 0,
                    NDOCID = entity.NDOCIDSpecified ? entity.NDOCID : null,
                    CHANGEDATE = entity.CHANGEDATE,
                    OriginalXMLString = source.OriginalXmlElement,
                    XmlFilePath = source.SourceFilePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка маппинга ChangeHistoryItem с CHANGEID: {CHANGEID} - OBJECTID: {OBJECTID}", source.Entity.CHANGEID, source.Entity.OBJECTID);
                throw;
            }
        }
    }
}
