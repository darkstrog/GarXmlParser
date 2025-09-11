using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ApartmentNodeMapper : IGarItemMapper<Apartment>
    {
        public string NodeName => "APARTMENT";

        public event Action<Apartment>? OnObjectMapped;

        public Apartment GetFromXelement(XElement element)
        {
            var apartment = new Apartment()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                NUMBER = (string)element.Attribute("NUMBER"),
                APARTTYPE = (string)element.Attribute("APARTTYPE"),
                OPERTYPEID = (long)element.Attribute("OPERTYPEID"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (APARTMENTSAPARTMENTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (APARTMENTSAPARTMENTISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"))
            };

            OnObjectMapped?.Invoke(apartment);

            return apartment;
        }
    }
}
