using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AddressObjectDivisionNodeMapper: IGarItemMapper<AddressObjectDivisionItem>
    {
        public string NodeName => "ITEM";

        public event Action<AddressObjectDivisionItem>? OnObjectMapped;
        public AddressObjectDivisionItem GetFromXelement(XElement element)
        {
            var addressObjectDivisionItem = new AddressObjectDivisionItem
            {
                ID = (long)element.Attribute("ID"),
                PARENTID = (long)element.Attribute("PARENTID"),
                CHILDID = (long)element.Attribute("CHILDID"),
                CHANGEID = (long)element.Attribute("CHANGEID")
            };

            OnObjectMapped?.Invoke(addressObjectDivisionItem);

            return addressObjectDivisionItem;
        }
    }
}
