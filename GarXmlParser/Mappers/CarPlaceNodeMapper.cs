using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class CarPlaceNodeMapper : IGarItemMapper<CarPlace>
    {
        public string NodeName => "CARPLACE";

        public event Action<IMappedObject<CarPlace>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<CarPlace>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            CarPlace carPlace = new CarPlace();
            string currentAttribute = "";

#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                carPlace.ID = (long)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                carPlace.OBJECTID = (long)element.Attribute("OBJECTID");

                currentAttribute = "OBJECTGUID";
                carPlace.OBJECTGUID = (string)element.Attribute("OBJECTGUID");

                currentAttribute = "CHANGEID";
                carPlace.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "NUMBER";
                carPlace.NUMBER = (string)element.Attribute("NUMBER");

                currentAttribute = "OPERTYPEID";
                carPlace.OPERTYPEID = (string)element.Attribute("OPERTYPEID");

                currentAttribute = "PREVID";
                carPlace.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                carPlace.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                carPlace.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                carPlace.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "UPDATEDATE";
                carPlace.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                carPlace.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                carPlace.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                carPlace.ISACTIVE = (CARPLACESCARPLACEISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "ISACTUAL";
                carPlace.ISACTUAL = (CARPLACESCARPLACEISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<CarPlace>
                {
                    Entity = carPlace,
                    OriginalXmlElement = element.ToString(),
                    SourceFilePath = fileName,
                    LineNumber = lineNumber
                };

                OnObjectMapped?.Invoke(result);

                return result;
            }
            catch(Exception ex)
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
