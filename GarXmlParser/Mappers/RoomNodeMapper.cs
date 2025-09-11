using GarXmlParser.GarEntities;
using System.Xml.Linq;
namespace GarXmlParser.Mappers
{
    public class RoomNodeMapper : IGarItemMapper<Room>
    {
        public string NodeName => "ROOM";

        public event Action<Room>? OnObjectMapped;

        public Room GetFromXelement(XElement element)
        {
            var room = new Room()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                NUMBER = (string)element.Attribute("NUMBER"),
                ROOMTYPE = (string)element.Attribute("ROOMTYPE"),
                OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ISACTIVE = (ROOMSROOMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (ROOMSROOMISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"))
            };

            OnObjectMapped?.Invoke(room);

            return room;
        }
    }
}
