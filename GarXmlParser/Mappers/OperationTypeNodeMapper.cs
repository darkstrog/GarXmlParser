using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class OperationTypeNodeMapper : IGarItemMapper<OperationType>
    {
        public string NodeName => "OPERATIONTYPE";
        public event Action<IMappedObject<OperationType>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;
        public IMappedObject<OperationType>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            OperationType operationType = new OperationType();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                operationType.ID = (string)element.Attribute("ID");

                currentAttribute = "NAME";
                operationType.NAME = (string)element.Attribute("NAME");

                currentAttribute = "SHORTNAME";
                operationType.SHORTNAME = (string)element.Attribute("SHORTNAME");

                currentAttribute = "DESC";
                operationType.DESC = (string)element.Attribute("DESC");

                currentAttribute = "STARTDATE";
                operationType.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "UPDATEDATE";
                operationType.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));

                currentAttribute = "ENDDATE";
                operationType.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));

                currentAttribute = "ISACTIVE";
                operationType.ISACTIVE = (bool)element.Attribute("ISACTIVE");

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<OperationType> result = new MappedObject<OperationType>
                {
                    Entity = operationType,
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
