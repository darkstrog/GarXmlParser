using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class RoomTypeNodeMapper : IGarItemMapper<RoomType>
    {
        public string NodeName => "ROOMTYPE";

        public event Action<IMappedObject<RoomType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<RoomType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            RoomType roomType = new RoomType();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "";
                roomType.ID = (string)element.Attribute("ID");
                currentAttribute = "";
                currentAttribute = "";
                roomType.NAME = (string)element.Attribute("NAME");
                currentAttribute = "";
                roomType.SHORTNAME = (string)element.Attribute("SHORTNAME");
                currentAttribute = "";
                roomType.DESC = (string)element.Attribute("DESC");
                currentAttribute = "";
                roomType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                currentAttribute = "";
                roomType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));
                currentAttribute = "";
                roomType.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                currentAttribute = "";
                roomType.ISACTIVE = (bool)element.Attribute("ISACTIVE");
#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<RoomType> result = new MappedObject<RoomType>
                {
                    Entity = roomType,
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
