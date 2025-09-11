using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocNodeMapper : IGarItemMapper<NormativeDoc>
    {
        public string NodeName => "NORMDOC";

        public event Action<NormativeDoc>? OnObjectMapped;

        public NormativeDoc GetFromXelement(XElement element)
        {
            var normDoc = new NormativeDoc()
            {
                ID = (long)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                DATE = DateTime.Parse((string)element.Attribute("DATE")),
                NUMBER = (string)element.Attribute("NUMBER"),
                TYPE = (string)element.Attribute("TYPE"),
                KIND = (string)element.Attribute("KIND"),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ORGNAME = (string)element.Attribute("ORGNAME"),
                REGNUM = (string)element.Attribute("REGNUM"),
                REGDATE = element.Attribute("REGDATE")?.Value is string REGDATEvalue &&
                                                      !string.IsNullOrEmpty(REGDATEvalue) &&
                                                      DateTime.TryParse(REGDATEvalue, out DateTime REGDATEresult)
                                                      ? REGDATEresult
                                                      : (DateTime?)null,
                REGDATESpecified = (string)element.Attribute("REGDATE") != null,
                ACCDATE = element.Attribute("ACCDATE")?.Value is string ACCDATEvalue &&
                                                      !string.IsNullOrEmpty(ACCDATEvalue) &&
                                                      DateTime.TryParse(ACCDATEvalue, out DateTime ACCDATEresult)
                                                      ? ACCDATEresult
                                                      : (DateTime?)null,
                ACCDATESpecified = (string)element.Attribute("ACCDATE") != null,
                COMMENT = (string)element.Attribute("COMMENT"),
            };

            OnObjectMapped?.Invoke(normDoc);

            return normDoc;
        }
    }
}
