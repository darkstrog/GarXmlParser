using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;
namespace GarXmlParser.Mappers
{
    public class RoomNodeMapper : IGarItemMapper<Room>
    {
        public string NodeName => "ROOM";

        public event Action<IMappedObject<Room>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<Room>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            Room room = new Room();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                room.ID = (long)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                room.OBJECTID = (long)element.Attribute("OBJECTID");

                currentAttribute = "OBJECTGUID";
                room.OBJECTGUID = (string)element.Attribute("OBJECTGUID");

                currentAttribute = "CHANGEID";
                room.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "NUMBER";
                room.NUMBER = (string)element.Attribute("NUMBER");

                currentAttribute = "ROOMTYPE";
                room.ROOMTYPE = (string)element.Attribute("ROOMTYPE");

                currentAttribute = "OPERTYPEID";
                room.OPERTYPEID = (string)element.Attribute("OPERTYPEID");

                currentAttribute = "PREVID";
                room.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                room.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                room.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                room.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "STARTDATE";
                room.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "UPDATEDATE";
                room.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "ENDDATE";
                room.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ISACTIVE";
                room.ISACTIVE = (ROOMSROOMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "ISACTUAL";
                room.ISACTUAL = (ROOMSROOMISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));
#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<Room> result = new MappedObject<Room>
                {
                    Entity = room,
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
