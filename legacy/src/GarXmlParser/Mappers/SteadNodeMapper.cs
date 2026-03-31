using GarXmlParser.GarEntities;
using GarXmlParser.Mappers.Helpers;
using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;
namespace GarXmlParser.Mappers
{
    public class SteadNodeMapper : IGarItemMapper<Stead>
    {
        public string NodeName => "STEAD";

        public event Action<IMappedObject<Stead>>? OnObjectMapped;
        public event Action<MappingError>? OnErrorMapping;

        public IMappedObject<Stead>? GetFromXelement(XElement element, string fileName, int lineNumber)
        {
            Stead stead = new Stead();
            string currentAttribute = "";
#pragma warning disable CS8604, CS8600, CS8601
            try
            {
                currentAttribute = "ID";
                stead.ID = (string)element.Attribute("ID");
                
                currentAttribute = "OBJECTID";
                stead.OBJECTID = (string)element.Attribute("OBJECTID");
                
                currentAttribute = "OBJECTGUID";
                stead.OBJECTGUID = (string)element.Attribute("OBJECTGUID");
               
                currentAttribute = "CHANGEID";
                stead.CHANGEID = (string)element.Attribute("CHANGEID");
                
                currentAttribute = "NUMBER";
                stead.NUMBER = (string)element.Attribute("NUMBER");
                
                currentAttribute = "OPERTYPEID";
                stead.OPERTYPEID = (string)element.Attribute("OPERTYPEID");
                
                currentAttribute = "PREVID";
                stead.PREVID = (string)element.Attribute("PREVID");
                
                currentAttribute = "NEXTID";
                stead.NEXTID = (string)element.Attribute("NEXTID");
                
                currentAttribute = "STARTDATE";
                stead.STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                
                currentAttribute = "UPDATEDATE";
                stead.UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE"));
                
                currentAttribute = "ENDDATE";
                stead.ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE"));
                
                currentAttribute = "ISACTIVE";
                stead.ISACTIVE = (STEADSSTEADISACTIVE)int.Parse((string)element.Attribute("ISACTIVE"));
                
                currentAttribute = "ISACTUAL";
                stead.ISACTUAL = (STEADSSTEADISACTUAL)int.Parse((string)element.Attribute("ISACTUAL"));

#pragma warning restore CS8604, CS8600, CS8601
                MappedObject<Stead> result = new MappedObject<Stead>
                {
                    Entity = stead,
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
