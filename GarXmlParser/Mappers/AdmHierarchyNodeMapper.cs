using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AdmHierarchyNodeMapper : IGarItemMapper<AdmHierarchy>
    {
        public string NodeName => "ITEM";

        public event Action<IMappedObject<AdmHierarchy>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
#pragma warning disable CS8604, CS8600, CS8601
        public IMappedObject<AdmHierarchy>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            AdmHierarchy admHierarchy = new AdmHierarchy();
            string currentAttribute = "";
            try
            {
                currentAttribute = "ID";
                admHierarchy.ID = (int)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                admHierarchy.OBJECTID = (int)element.Attribute("OBJECTID");

                currentAttribute = "PARENTOBJID";
                admHierarchy.PARENTOBJID = (long)element.Attribute("PARENTOBJID");
                
                currentAttribute = "PARENTOBJIDSpecified";
                admHierarchy.PARENTOBJIDSpecified = element.Attribute("PARENTOBJID") != null;

                currentAttribute = "CHANGEID";
                admHierarchy.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "REGIONCODE";
                admHierarchy.REGIONCODE = (string)element.Attribute("REGIONCODE");

                currentAttribute = "AREACODE";
                admHierarchy.AREACODE = (string)element.Attribute("AREACODE");

                currentAttribute = "CITYCODE";
                admHierarchy.CITYCODE = (string)element.Attribute("CITYCODE");

                currentAttribute = "PLACECODE";
                admHierarchy.PLACECODE = (string)element.Attribute("PLACECODE");

                currentAttribute = "PLANCODE";
                admHierarchy.PLANCODE = (string)element.Attribute("PLANCODE");

                currentAttribute = "STREETCODE";
                admHierarchy.STREETCODE = (string)element.Attribute("STREETCODE");

                currentAttribute = "PREVID";
                admHierarchy.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                admHierarchy.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                admHierarchy.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                admHierarchy.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "UPDATEDATE";
                admHierarchy.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                admHierarchy.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                admHierarchy.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "ISACTIVE";
                admHierarchy.ISACTIVE = (AdmHierarchyITEMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

                currentAttribute = "PATH";
                admHierarchy.PATH = (string)element.Attribute("PATH");
#pragma warning restore CS8604, CS8600, CS8601

                var result = new MappedObject<AdmHierarchy>
            {
                Entity = admHierarchy,
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
