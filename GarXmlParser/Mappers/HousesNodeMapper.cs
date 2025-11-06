using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
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

        public event Action<IMappedObject<HOUSESHOUSE>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<HOUSESHOUSE>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            HOUSESHOUSE house = new HOUSESHOUSE();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                house.ID = (long)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                house.OBJECTID = (long)element.Attribute("OBJECTID");

                currentAttribute = "OBJECTGUID";
                house.OBJECTGUID = (string)element.Attribute("OBJECTGUID");

                currentAttribute = "CHANGEID";
                house.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "HOUSENUM";
                house.HOUSENUM = (string)element.Attribute("HOUSENUM");

                currentAttribute = "ADDNUM1";
                house.ADDNUM1 = (string)element.Attribute("ADDNUM1");

                currentAttribute = "ADDNUM2";
                house.ADDNUM2 = (string)element.Attribute("ADDNUM2");

                currentAttribute = "HOUSETYPE";
                house.HOUSETYPE = (string)element.Attribute("HOUSETYPE");

                currentAttribute = "ADDTYPE1";
                house.ADDTYPE1 = (string)element.Attribute("ADDTYPE1");

                currentAttribute = "ADDTYPE2";
                house.ADDTYPE2 = (string)element.Attribute("ADDTYPE2");

                currentAttribute = "OPERTYPEID";
                house.OPERTYPEID = (string)element.Attribute("OPERTYPEID");

                currentAttribute = "PREVID";
                house.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                house.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                house.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                house.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "UPDATEDATE";
                house.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                house.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                house.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                house.ISACTIVE = (HOUSESHOUSEISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "ISACTUAL";
                house.ISACTUAL = (HOUSESHOUSEISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<HOUSESHOUSE>
                {
                    Entity = house,
                    OriginalXmlElement = element.ToString(),
                    SourceFilePath = fileName,
                    LineNumber = lineNumber
                };

                OnObjectMapped?.Invoke(result);

                return result;
            }
            catch (Exception ex)
            {
                MappingError mappingError = new MappingError
                {
                    Exception = ex,
                    OriginalXmlElement = element.ToString(),
                    FileName = fileName,
                    LineNumber = lineNumber,
                    AttributeName = currentAttribute,
                    ErrorTime = DateTime.Now
                };

                OnErrorMapping?.Invoke(mappingError);

                return null;
            }
        }
    }
}
