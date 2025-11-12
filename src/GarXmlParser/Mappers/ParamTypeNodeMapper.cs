using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ParamTypeNodeMapper : IGarItemMapper<ParamType>
    {
        public string NodeName => "PARAMTYPE";

        public event Action<IMappedObject<ParamType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<ParamType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ParamType paramType = new ParamType();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                paramType.ID = (string)element.Attribute("ID");
                
                currentAttribute = "NAME"; 
                paramType.NAME = (string)element.Attribute("NAME");
                
                currentAttribute = "CODE"; 
                paramType.CODE = (string)element.Attribute("CODE");
                
                currentAttribute = "DESC"; 
                paramType.DESC = (string)element.Attribute("DESC");
                
                currentAttribute = "STARTDATE"; 
                paramType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                
                currentAttribute = "UPDATEDATE"; 
                paramType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));
                
                currentAttribute = "ENDDATE"; 
                paramType.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                
                currentAttribute = "ISACTIVE"; 
                paramType.ISACTIVE = (bool)element.Attribute("ISACTIVE");

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<ParamType> result = new MappedObject<ParamType>
                {
                    Entity = paramType,
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
