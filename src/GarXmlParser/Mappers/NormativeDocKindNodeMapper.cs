using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class NormativeDocKindNodeMapper : IGarItemMapper<NormativeDocKind>
    {
        public string NodeName => "NDOCKIND";

        public event Action<IMappedObject<NormativeDocKind>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<NormativeDocKind>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            NormativeDocKind normDocKind = new NormativeDocKind();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                normDocKind.ID = (string)element.Attribute("ID");

                currentAttribute = "NAME";
                normDocKind.NAME = (string)element.Attribute("NAME");

                MappedObject<NormativeDocKind> result = new MappedObject<NormativeDocKind>
                {
                    Entity = normDocKind,
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
#pragma warning restore CS8604, CS8600, CS8601
        }
    }
}
