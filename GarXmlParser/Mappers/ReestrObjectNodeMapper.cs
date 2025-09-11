using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ReestrObjectNodeMapper : IGarItemMapper<ReestrObject>
    {
        public string NodeName => "OBJECT";

        public event Action<ReestrObject>? OnObjectMapped;

        public ReestrObject GetFromXelement(XElement element)
        {
            var reestrObject = new ReestrObject()
            {
                OBJECTID = (long)element.Attribute("OBJECTID"),
                CREATEDATE = DateTime.Parse((string)element.Attribute("CREATEDATE")),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                LEVELID = (string)element.Attribute("LEVELID"),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                ISACTIVE = (REESTR_OBJECTSOBJECTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"))
            };

            OnObjectMapped?.Invoke(reestrObject);

            return reestrObject;
        }
    }
}
