using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocKindNodeMapper : IGarItemMapper<NormativeDocKind>
    {
        public string NodeName => "NDOCKIND";

        public event Action<NormativeDocKind>? OnObjectMapped;

        public NormativeDocKind GetFromXelement(XElement element)
        {
            var normDocKind = new NormativeDocKind()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
            };

            OnObjectMapped?.Invoke(normDocKind);

            return normDocKind;
        }
    }
}
