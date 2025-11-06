using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocNodeMapper : IGarItemMapper<NormativeDoc>
    {
        public string NodeName => "NORMDOC";

        public event Action<IMappedObject<NormativeDoc>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<NormativeDoc>? GetFromXelement(XElement element,string fileName, int lineNumber)
        {
            NormativeDoc normDoc = new NormativeDoc();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                normDoc.ID = (long)element.Attribute("ID");

                currentAttribute = "NAME";
                normDoc.NAME = (string)element.Attribute("NAME");

                currentAttribute = "DATE";
                normDoc.DATE = DateTime.Parse((string)element.Attribute("DATE"));

                currentAttribute = "NUMBER";
                normDoc.NUMBER = (string)element.Attribute("NUMBER");

                currentAttribute = "TYPE";
                normDoc.TYPE = (string)element.Attribute("TYPE");

                currentAttribute = "KIND";
                normDoc.KIND = (string)element.Attribute("KIND");

                currentAttribute = "UPDATEDATE";
                normDoc.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "ORGNAME";
                normDoc.ORGNAME = (string)element.Attribute("ORGNAME");

                currentAttribute = "REGNUM";
                normDoc.REGNUM = (string)element.Attribute("REGNUM");

                currentAttribute = "REGDATE";
                normDoc.REGDATE = element.Attribute("REGDATE")?.Value is string REGDATEvalue &&
                                                      !string.IsNullOrEmpty(REGDATEvalue) &&
                                                      DateTime.TryParse(REGDATEvalue, out DateTime REGDATEresult)
                                                      ? REGDATEresult
                                                      : (DateTime?)null;

                currentAttribute = "REGDATESpecified";
                normDoc.REGDATESpecified = (string)element.Attribute("REGDATE") != null;

                currentAttribute = "ACCDATE";
                normDoc.ACCDATE = element.Attribute("ACCDATE")?.Value is string ACCDATEvalue &&
                                                      !string.IsNullOrEmpty(ACCDATEvalue) &&
                                                      DateTime.TryParse(ACCDATEvalue, out DateTime ACCDATEresult)
                                                      ? ACCDATEresult
                                                      : (DateTime?)null;

                currentAttribute = "ACCDATESpecified";
                normDoc.ACCDATESpecified = (string)element.Attribute("ACCDATE") != null;

                currentAttribute = "COMMENT";
                normDoc.COMMENT = (string)element.Attribute("COMMENT");

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<NormativeDoc> result = new MappedObject<NormativeDoc>
                {
                    Entity = normDoc,
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
