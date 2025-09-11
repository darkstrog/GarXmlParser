using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AdmHierarchyNodeMapper : IGarItemMapper<AdmHierarchy>
    {
        public string NodeName => "ITEM";

        public event Action<AdmHierarchy>? OnObjectMapped;

        public AdmHierarchy GetFromXelement(XElement element)
        {
            var admHierarchy = new AdmHierarchy()
            {
                ID = (int)element.Attribute("ID"),
                OBJECTID = (int)element.Attribute("OBJECTID"),
                PARENTOBJID = (long)element.Attribute("PARENTOBJID"),
                PARENTOBJIDSpecified = element.Attribute("PARENTOBJID") != null,
                CHANGEID = (long)element.Attribute("CHANGEID"),
                REGIONCODE = (string)element.Attribute("REGIONCODE"),
                AREACODE = (string)element.Attribute("AREACODE"),
                CITYCODE = (string)element.Attribute("CITYCODE"),
                PLACECODE = (string)element.Attribute("PLACECODE"),
                PLANCODE = (string)element.Attribute("PLANCODE"),
                STREETCODE = (string)element.Attribute("STREETCODE"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (AdmHierarchyITEMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                PATH = (string)element.Attribute("PATH")
            };

            OnObjectMapped?.Invoke(admHierarchy);

            return admHierarchy;
        }
    }
}
