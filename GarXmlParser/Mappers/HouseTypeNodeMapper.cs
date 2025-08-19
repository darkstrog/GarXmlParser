using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class HouseTypeNodeMapper: IGarItemMapper<HOUSETYPESHOUSETYPE>
    {
        public string NodeName => "HOUSETYPE";

        public event Action<HOUSETYPESHOUSETYPE> OnObjectMapped;
        public HOUSETYPESHOUSETYPE GetFromXelement(XElement element)
        {
            var houseType =  new HOUSETYPESHOUSETYPE()
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
            
            OnObjectMapped.Invoke(houseType);
            
            return houseType;
        }
    }
}
