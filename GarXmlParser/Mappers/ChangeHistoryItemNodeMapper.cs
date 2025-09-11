using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ChangeHistoryItemNodeMapper : IGarItemMapper<ChangeHistoryItem>
    {
        public string NodeName => "ITEM";

        public event Action<ChangeHistoryItem>? OnObjectMapped;

        public ChangeHistoryItem GetFromXelement(XElement element)
        {

            try
            {
                var changeHistoryItem = new ChangeHistoryItem()
                {
                    CHANGEID = (long)element.Attribute("CHANGEID"),
                    OBJECTID = (long)element.Attribute("OBJECTID"),
                    ADROBJECTID = (string)element.Attribute("ADROBJECTID"),
                    OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                    NDOCID = (long?)element.Attribute("NDOCID") ?? 0,
                    NDOCIDSpecified = element.Attribute("NDOCID") != null,
                    CHANGEDATE = DateTime.Parse((string)element.Attribute("CHANGEDATE")),
                };
            OnObjectMapped?.Invoke(changeHistoryItem);

            return changeHistoryItem;
            }
            catch (Exception)
            {
                Console.WriteLine($"{element.Attribute("CHANGEID")}--{element.Attribute("OBJECTID")}--{element.Attribute("ADROBJECTID")}--{element.Attribute("OPERTYPEID")}--{element.Attribute("NDOCID")}--{element.Attribute("CHANGEDATE")}");
                throw;
            }
            

            
        }
    }
}
