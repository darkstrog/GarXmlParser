using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AddressObjectTypeNodeMapper : IGarItemMapper<AddressObjectType>
    {
        public string NodeName => "ADDRESSOBJECTTYPE";

        public event Action<IMappedObject<AddressObjectType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
#pragma warning disable CS8604, CS8600, CS8601
        public IMappedObject<AddressObjectType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            AddressObjectType addressObjectType = new AddressObjectType();
            string currentAttribute="";
            try
            {
                currentAttribute = "ID";
                addressObjectType.ID = (string)element.Attribute("ID");

                currentAttribute = "LEVEL";
                addressObjectType.LEVEL = (string)element.Attribute("LEVEL");

                currentAttribute = "SHORTNAME";
                addressObjectType.SHORTNAME = (string)element.Attribute("SHORTNAME");

                currentAttribute = "NAME";
                addressObjectType.NAME = (string)element.Attribute("NAME");

                currentAttribute = "DESC";
                addressObjectType.DESC = (string)element.Attribute("DESC");

                currentAttribute = "UPDATEDATE";
                addressObjectType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                addressObjectType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                addressObjectType.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                addressObjectType.ISACTIVE = bool.Parse((string)element.Attribute("ISACTIVE"));

#pragma warning restore CS8604, CS8600, CS8601

                var result = new MappedObject<AddressObjectType>
                {
                    Entity = addressObjectType,
                    OriginalXmlElement = element.ToString(),
                    SourceFilePath = fileName,
                    LineNumber = lineNumber
                };

                OnObjectMapped?.Invoke(result);

                return result;
            }
            catch(Exception ex)
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
