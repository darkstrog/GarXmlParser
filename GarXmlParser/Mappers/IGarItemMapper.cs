using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    internal interface IGarItemMapper<T>
    {
        string NodeName { get; }
        event Action<T> OnObjectMapped;
        T GetFromXelement(XElement element);
    }
}
