using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ParamNodeMapper : IGarItemMapper<Param>
    {
        public string NodeName => "PARAM";

        public event Action<IMappedObject<Param>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<Param>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            Param param = new Param();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID"; 
                param.ID = (long)element.Attribute("ID");
                
                currentAttribute = "OBJECTID"; 
                param.OBJECTID = (long)element.Attribute("OBJECTID");
                
                currentAttribute = "CHANGEID"; 
                param.CHANGEID = (long?)element.Attribute("CHANGEID") ?? 0;
                
                currentAttribute = "CHANGEIDSpecified"; 
                param.CHANGEIDSpecified = element.Attribute("NEXTID") != null;
                
                currentAttribute = "CHANGEIDEND"; 
                param.CHANGEIDEND = (long)element.Attribute("CHANGEIDEND");
                
                currentAttribute = "TYPEID"; 
                param.TYPEID = (string)element.Attribute("TYPEID");
                
                currentAttribute = "VALUE"; 
                param.VALUE = (string)element.Attribute("VALUE");
                
                currentAttribute = "STARTDATE"; 
                param.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                
                currentAttribute = "UPDATEDATE"; 
                param.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));
                
                currentAttribute = "ENDDATE"; 
                param.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<Param> result = new MappedObject<Param>
                {
                    Entity = param,
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
