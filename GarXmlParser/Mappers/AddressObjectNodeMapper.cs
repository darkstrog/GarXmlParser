using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AddressObjectNodeMapper: IGarItemMapper<ADDRESSOBJECTSOBJECT>
    {
        public string NodeName => "OBJECT";

        public event Action<IMappedObject<ADDRESSOBJECTSOBJECT>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

#pragma warning disable CS8604, CS8600
        public IMappedObject<ADDRESSOBJECTSOBJECT>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            ADDRESSOBJECTSOBJECT addressObject = new ADDRESSOBJECTSOBJECT();
#pragma warning disable CS8604, CS8600, CS8601
            string currentAttribute = "";
            try
            {
                currentAttribute = "ID";
                addressObject.ID = (int)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                addressObject.OBJECTID = (int)element.Attribute("OBJECTID");

                currentAttribute = "OBJECTGUID";
                addressObject.OBJECTGUID = (string)element.Attribute("OBJECTGUID");

                currentAttribute = "NAME";
                addressObject.NAME = (string)element.Attribute("NAME");

                currentAttribute = "CHANGEID";
                addressObject.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "UPDATEDATE";
                addressObject.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "ENDDATE";
                addressObject.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                addressObject.ISACTIVE = (ADDRESSOBJECTSOBJECTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "ISACTUAL";
                addressObject.ISACTUAL = (ADDRESSOBJECTSOBJECTISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));

                currentAttribute = "LEVEL";
                addressObject.LEVEL = (string)element.Attribute("LEVEL");

                currentAttribute = "NEXTID";
                addressObject.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                addressObject.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "PREVID";
                addressObject.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                addressObject.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "OPERTYPEID";
                addressObject.OPERTYPEID = (string)element.Attribute("OPERTYPEID");

                currentAttribute = "STARTDATE";
                addressObject.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "TYPENAME";
                addressObject.TYPENAME = (string)element.Attribute("TYPENAME");

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<ADDRESSOBJECTSOBJECT>
                {
                    Entity = addressObject,
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

