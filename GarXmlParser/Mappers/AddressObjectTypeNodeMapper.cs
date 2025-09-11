using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GarXmlParser.GarEntities;

namespace GarXmlParser.Mappers
{
    public class AddressObjectTypeNodeMapper : IGarItemMapper<AddressObjectType>
    {
        public string NodeName => "ADDRESSOBJECTTYPE";

        public event Action<AddressObjectType>? OnObjectMapped;

        public AddressObjectType GetFromXelement(XElement element)
        {
            var addressObjectType = new AddressObjectType()
            {
                ID = (string)element.Attribute("ID"),
                LEVEL = (string)element.Attribute("LEVEL"),
                SHORTNAME = (string)element.Attribute("SHORTNAME"),
                NAME = (string)element.Attribute("NAME"),
                DESC = (string)element.Attribute("DESC"),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = bool.Parse((string)element.Attribute("ISACTIVE"))
            };

            OnObjectMapped?.Invoke(addressObjectType);

            return addressObjectType;
        }
    }
}
