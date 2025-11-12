using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocTypeNodeMapper : IGarItemMapper<NormativeDocType>
    {
        public string NodeName => "NDOCTYPE";

        public event Action<IMappedObject<NormativeDocType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<NormativeDocType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            NormativeDocType normDocType = new NormativeDocType();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "";
                normDocType.ID = (string)element.Attribute("ID");

                currentAttribute = "";
                normDocType.NAME = (string)element.Attribute("NAME");

                currentAttribute = "";
                normDocType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "";
                normDocType.ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE"));

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<NormativeDocType> result = new MappedObject<NormativeDocType>
                {
                    Entity = normDocType,
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
