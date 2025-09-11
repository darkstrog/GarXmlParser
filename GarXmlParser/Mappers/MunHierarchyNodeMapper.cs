using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class MunHierarchyNodeMapper : IGarItemMapper<MunHierarchy>
    {
        public string NodeName => "ITEM";

        public event Action<MunHierarchy>? OnObjectMapped;

        public MunHierarchy GetFromXelement(XElement element)
        {
            var munHierarchy = new MunHierarchy()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                PARENTOBJID = (long?)element.Attribute("PARENTOBJID")?? 0,
                PARENTOBJIDSpecified = (string)element.Attribute("PARENTOBJID") != null,
                CHANGEID = (long)element.Attribute("CHANGEID"),
                OKTMO = (string)element.Attribute("OKTMO"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                PATH = (string)element.Attribute("PATH"),
                ISACTIVE = (ITEMSITEMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"))
            };

            OnObjectMapped?.Invoke(munHierarchy);

            return munHierarchy;
        }
    }
}
