using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ParamNodeMapper : IGarItemMapper<Param>
    {
        public string NodeName => "PARAM";

        public event Action<Param>? OnObjectMapped;

        public Param GetFromXelement(XElement element)
        {
            var param = new Param()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                CHANGEID = (long?)element.Attribute("CHANGEID") ?? 0,
                CHANGEIDSpecified = element.Attribute("NEXTID") != null,
                CHANGEIDEND = (long)element.Attribute("CHANGEIDEND"),
                TYPEID = (string)element.Attribute("TYPEID"),
                VALUE = (string)element.Attribute("VALUE"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"))
            };

            OnObjectMapped?.Invoke(param);

            return param;
        }
    }
}
