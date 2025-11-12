using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ObjectLevelNodeMapper : IGarItemMapper<ObjectLevel>
    {
        public string NodeName => "OBJECTLEVEL";

        public event Action<IMappedObject<ObjectLevel>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<ObjectLevel>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ObjectLevel objectLevel = new ObjectLevel();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "LEVEL";
                objectLevel.LEVEL = (string)element.Attribute("LEVEL");

                currentAttribute = "NAME";
                objectLevel.NAME = (string)element.Attribute("NAME");

                currentAttribute = "SHORTNAME";
                objectLevel.SHORTNAME = (string)element.Attribute("SHORTNAME");

                currentAttribute = "STARTDATE";
                objectLevel.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "UPDATEDATE";
                objectLevel.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "ENDDATE";
                objectLevel.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                objectLevel.ISACTIVE = (bool)element.Attribute("ISACTIVE");

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<ObjectLevel> result = new MappedObject<ObjectLevel>
                {
                    Entity = objectLevel,
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
