using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class MunHierarchyNodeMapper : IGarItemMapper<MunHierarchy>
    {
        public string NodeName => "ITEM";

        public event Action<IMappedObject<MunHierarchy>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<MunHierarchy>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            MunHierarchy munHierarchy = new MunHierarchy();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                munHierarchy.ID = (long)element.Attribute("ID");

                currentAttribute = "OBJECTID";
                munHierarchy.OBJECTID = (long)element.Attribute("OBJECTID");

                currentAttribute = "PARENTOBJID";
                munHierarchy.PARENTOBJID = (long?)element.Attribute("PARENTOBJID") ?? 0;

                currentAttribute = "PARENTOBJIDSpecified";
                munHierarchy.PARENTOBJIDSpecified = (string)element.Attribute("PARENTOBJID") != null;

                currentAttribute = "CHANGEID";
                munHierarchy.CHANGEID = (long)element.Attribute("CHANGEID");

                currentAttribute = "OKTMO";
                munHierarchy.OKTMO = (string)element.Attribute("OKTMO");

                currentAttribute = "PREVID";
                munHierarchy.PREVID = (long?)element.Attribute("PREVID") ?? 0;

                currentAttribute = "PREVIDSpecified";
                munHierarchy.PREVIDSpecified = (string)element.Attribute("PREVID") != null;

                currentAttribute = "NEXTID";
                munHierarchy.NEXTID = (long?)element.Attribute("NEXTID") ?? 0;

                currentAttribute = "NEXTIDSpecified";
                munHierarchy.NEXTIDSpecified = element.Attribute("NEXTID") != null;

                currentAttribute = "UPDATEDATE";
                munHierarchy.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "STARTDATE";
                munHierarchy.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ENDDATE";
                munHierarchy.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

                currentAttribute = "PATH";
                munHierarchy.PATH = (string)element.Attribute("PATH");

                currentAttribute = "ISACTIVE";
                munHierarchy.ISACTIVE = (ITEMSITEMISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));

#pragma warning restore CS8604, CS8600, CS8601
                var result = new MappedObject<MunHierarchy>
                {
                    Entity = munHierarchy,
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
