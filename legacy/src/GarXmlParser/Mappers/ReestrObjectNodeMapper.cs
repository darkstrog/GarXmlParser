using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ReestrObjectNodeMapper : IGarItemMapper<ReestrObject>
    {
        public string NodeName => "OBJECT";

        public event Action<IMappedObject<ReestrObject>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<ReestrObject>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ReestrObject reestrObject = new ReestrObject();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "OBJECTID";
                reestrObject.OBJECTID = (long)element.Attribute("OBJECTID");
                
                currentAttribute = "CREATEDATE"; 
                reestrObject.CREATEDATE = DateTime.Parse((string)element.Attribute("CREATEDATE"));
                
                currentAttribute = "CHANGEID"; 
                reestrObject.CHANGEID = (long)element.Attribute("CHANGEID");
                
                currentAttribute = "LEVELID"; 
                reestrObject.LEVELID = (string)element.Attribute("LEVELID");
                
                currentAttribute = "UPDATEDATE";
                reestrObject.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));
                
                currentAttribute = "OBJECTGUID"; 
                reestrObject.OBJECTGUID = (string)element.Attribute("OBJECTGUID");
                
                currentAttribute = "ISACTIVE"; 
                reestrObject.ISACTIVE = (REESTR_OBJECTSOBJECTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<ReestrObject> result = new MappedObject<ReestrObject>
                {
                    Entity = reestrObject,
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
