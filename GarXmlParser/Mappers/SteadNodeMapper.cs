using GarXmlParser.GarEntities;
using System.Xml.Linq;
namespace GarXmlParser.Mappers
{
    public class SteadNodeMapper : IGarItemMapper<Stead>
    {
        public string NodeName => "STEAD";

        public event Action<Stead>? OnObjectMapped;

        public Stead GetFromXelement(XElement element)
        {
            var stead = new Stead()
            {
                ID = (string)element.Attribute("ID"),
                OBJECTID = (string)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                CHANGEID = (string)element.Attribute("CHANGEID"),
                NUMBER = (string)element.Attribute("NUMBER"),
                OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                PREVID = (string)element.Attribute("PREVID"),
                NEXTID = (string)element.Attribute("NEXTID"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ISACTIVE = (STEADSSTEADISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (STEADSSTEADISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"))
            };

            OnObjectMapped?.Invoke(stead);

            return stead;
        }
    }
}
