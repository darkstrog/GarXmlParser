using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class RoomTypeNodeMapper : IGarItemMapper<RoomType>
    {
        public string NodeName => "ROOMTYPE";

        public event Action<RoomType>? OnObjectMapped;

        public RoomType GetFromXelement(XElement element)
        {
            var roomType = new RoomType()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                SHORTNAME = (string)element.Attribute("SHORTNAME"),
                DESC = (string)element.Attribute("DESC"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ISACTIVE = (bool)element.Attribute("ISACTIVE")
            };

            OnObjectMapped?.Invoke(roomType);

            return roomType;
        }
    }
}
