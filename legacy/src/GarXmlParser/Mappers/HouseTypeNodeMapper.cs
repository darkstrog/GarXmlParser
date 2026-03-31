using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class HouseTypeNodeMapper: IGarItemMapper<HOUSETYPESHOUSETYPE>
    {
        public string NodeName => "HOUSETYPE";

        public event Action<IMappedObject<HOUSETYPESHOUSETYPE>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<HOUSETYPESHOUSETYPE>? GetFromXelement(XElement element, string fileName, int lineNumber)
        { 
            HOUSETYPESHOUSETYPE houseType = new HOUSETYPESHOUSETYPE();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "";
                houseType.ID = (string)element.Attribute("ID");

                currentAttribute = "";
                houseType.NAME = (string)element.Attribute("NAME");

                currentAttribute = "";
                houseType.SHORTNAME = (string)element.Attribute("SHORTNAME");

                currentAttribute = "";
                houseType.DESC = (string)element.Attribute("DESC");

                currentAttribute = "";
                houseType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "";
                houseType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "";
                houseType.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "";
                houseType.ISACTIVE = bool.Parse((string)element.Attribute("ISACTIVE"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<HOUSETYPESHOUSETYPE>
                {
                    Entity = houseType,
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
