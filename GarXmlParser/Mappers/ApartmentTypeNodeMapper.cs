using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ApartmentTypeNodeMapper : IGarItemMapper<ApartmentType>
    {
        public string NodeName => "APARTMENTTYPE";

        public event Action<IMappedObject<ApartmentType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<ApartmentType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ApartmentType apartmentType = new ApartmentType();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "";
                apartmentType.ID = (string)element.Attribute("ID");

                currentAttribute = "";
                apartmentType.NAME = (string)element.Attribute("NAME");

                currentAttribute = "";
                apartmentType.SHORTNAME = (string)element.Attribute("SHORTNAME");
                
                currentAttribute = "";
                apartmentType.DESC = (string)element.Attribute("DESC");

                currentAttribute = "";
                apartmentType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "";
                apartmentType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "";
                apartmentType.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "";
                apartmentType.ISACTIVE = bool.Parse((string)element.Attribute("ISACTIVE"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<ApartmentType>
                {
                    Entity = apartmentType,
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
