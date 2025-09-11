using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocTypeNodeMapper : IGarItemMapper<NormativeDocType>
    {
        public string NodeName => "NDOCTYPE";

        public event Action<NormativeDocType>? OnObjectMapped;

        public NormativeDocType GetFromXelement(XElement element)
        {
            var normDocType = new NormativeDocType()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"))
            };

            OnObjectMapped?.Invoke(normDocType);

            return normDocType;
        }
    }
}
