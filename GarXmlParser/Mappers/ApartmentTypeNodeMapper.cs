using GarXmlParser.GarEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ApartmentTypeNodeMapper : IGarItemMapper<ApartmentType>
    {
        public string NodeName => "APARTMENTTYPE";

        public event Action<ApartmentType>? OnObjectMapped;

        public ApartmentType GetFromXelement(XElement element)
        {
            var apartmentType = new ApartmentType()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                SHORTNAME = (string)element.Attribute("SHORTNAME"),
                DESC = (string)element.Attribute("DESC"),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = bool.Parse((string)element.Attribute("ISACTIVE"))
            };

            OnObjectMapped?.Invoke(apartmentType);

            return apartmentType;
        }
    }
}
