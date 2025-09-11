using GarXmlParser.GarEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class CarPlaceNodeMapper : IGarItemMapper<CarPlace>
    {
        public string NodeName => "CARPLACE";

        public event Action<CarPlace>? OnObjectMapped;

        public CarPlace GetFromXelement(XElement element)
        {
            var carPlace = new CarPlace()
            {
                ID = (long)element.Attribute("ID"),
                OBJECTID = (long)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                NUMBER = (string)element.Attribute("NUMBER"),
                OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (CARPLACESCARPLACEISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (CARPLACESCARPLACEISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"))
            };

            OnObjectMapped?.Invoke(carPlace);

            return carPlace;
        }
    }
}
