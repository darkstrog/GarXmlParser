using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.IO.Enumeration;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ApartmentNodeMapper : IGarItemMapper<Apartment>
    {
        public string NodeName => "APARTMENT";

        public event Action<IMappedObject<Apartment>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

#pragma warning disable CS8604, CS8600, CS8601
        public IMappedObject<Apartment>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            Apartment apartment = new Apartment();
            string currentAttribute = "";
            try
            {
                currentAttribute = "ID";
                apartment.ID = (long)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                apartment.OBJECTID = (long)element.Attribute("OBJECTID");

                currentAttribute = "OBJECTGUID";
                apartment.OBJECTGUID = (string)element.Attribute("OBJECTGUID");

                currentAttribute = "CHANGEID";
                apartment.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "NUMBER";
                apartment.NUMBER = (string)element.Attribute("NUMBER"); 
                
                currentAttribute = "APARTTYPE";
                apartment.APARTTYPE = (string)element.Attribute("APARTTYPE");

                currentAttribute = "OPERTYPEID";
                apartment.OPERTYPEID = (long)element.Attribute("OPERTYPEID");

                currentAttribute = "PREVID";
                apartment.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                apartment.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                apartment.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                apartment.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "UPDATEDATE";
                apartment.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                apartment.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                apartment.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                apartment.ISACTIVE = (APARTMENTSAPARTMENTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "ISACTUAL";
                apartment.ISACTUAL = (APARTMENTSAPARTMENTISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<Apartment>
            {
                Entity = apartment,
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
