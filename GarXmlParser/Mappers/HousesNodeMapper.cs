using GarXmlParser.GarEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class HousesNodeMapper : IGarItemMapper<HOUSESHOUSE>
    {
        public string NodeName => "HOUSE";

        public event Action<HOUSESHOUSE>? OnObjectMapped;

        public HOUSESHOUSE GetFromXelement(XElement element)
        {
            var house = new HOUSESHOUSE()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                HOUSENUM = (string)element.Attribute("HOUSENUM"),
                ADDNUM1 = (string)element.Attribute("ADDNUM1"),
                ADDNUM2 = (string)element.Attribute("ADDNUM2"),
                HOUSETYPE = (string)element.Attribute("HOUSETYPE"),
                ADDTYPE1 = (string)element.Attribute("ADDTYPE1"),
                ADDTYPE2 = (string)element.Attribute("ADDTYPE2"),
                OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (HOUSESHOUSEISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (HOUSESHOUSEISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"))
            };

            OnObjectMapped?.Invoke(house);

            return house;
        }
    }
}
