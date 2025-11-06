using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AddressObjectDivisionNodeMapper: IGarItemMapper<AddressObjectDivisionItem>
    {
        public string NodeName => "ITEM";

        public event Action<IMappedObject<AddressObjectDivisionItem>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

#pragma warning disable CS8604, CS8600
        public IMappedObject<AddressObjectDivisionItem>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            AddressObjectDivisionItem addressObjectDivisionItem = new AddressObjectDivisionItem();
            
            string currentAttribute = "";
            
            try
            {
                currentAttribute = "ID";
                addressObjectDivisionItem.ID = (long)element.Attribute("ID");

                currentAttribute = "PARENTID";
                addressObjectDivisionItem.PARENTID = (long)element.Attribute("PARENTID");

                currentAttribute = "CHILDID";
                addressObjectDivisionItem.CHILDID = (long)element.Attribute("CHILDID");

                currentAttribute = "CHANGEID";
                addressObjectDivisionItem.CHANGEID = (long)element.Attribute("CHANGEID");

#pragma warning restore CS8604, CS8600
                var result = new MappedObject<AddressObjectDivisionItem>

                {
                    Entity = addressObjectDivisionItem,
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
