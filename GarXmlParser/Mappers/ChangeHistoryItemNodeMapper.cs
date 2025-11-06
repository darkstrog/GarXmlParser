using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ChangeHistoryItemNodeMapper : IGarItemMapper<ChangeHistoryItem>
    {
        public string NodeName => "ITEM";

        public event Action<IMappedObject<ChangeHistoryItem>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<ChangeHistoryItem>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ChangeHistoryItem changeHistoryItem = new ChangeHistoryItem();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "CHANGEID";
                changeHistoryItem.CHANGEID = (long)element.Attribute("CHANGEID");
                
                currentAttribute = "OBJECTID";
                changeHistoryItem.OBJECTID = (long)element.Attribute("OBJECTID");
                
                currentAttribute = "ADROBJECTID";
                changeHistoryItem.ADROBJECTID = (string)element.Attribute("ADROBJECTID");

                currentAttribute = "OPERTYPEID";
                changeHistoryItem.OPERTYPEID = (string)element.Attribute("OPERTYPEID");

                currentAttribute = "NDOCID";
                changeHistoryItem.NDOCID = (long?)element.Attribute("NDOCID") ?? 0;

                currentAttribute = "NDOCIDSpecified";
                changeHistoryItem.NDOCIDSpecified = element.Attribute("NDOCID") != null;

                currentAttribute = "CHANGEDATE";
                changeHistoryItem.CHANGEDATE = DateTime.Parse((string)element.Attribute("CHANGEDATE"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<ChangeHistoryItem>
                {
                    Entity = changeHistoryItem,
                    OriginalXmlElement = element.ToString(),
                    SourceFilePath = fileName,
                    LineNumber = lineNumber
                };

                OnObjectMapped?.Invoke(result);

                return result;
            }
            catch (Exception ex)
            {
                MappingError mappingError = new MappingError
                {
                    Exception = ex,
                    OriginalXmlElement = element.ToString(),
                    FileName = fileName,
                    LineNumber = lineNumber,
                    AttributeName = currentAttribute,
                    ErrorTime = DateTime.Now
                };

                OnErrorMapping?.Invoke(mappingError);

                return null;
            }
            

            
        }
    }
}
