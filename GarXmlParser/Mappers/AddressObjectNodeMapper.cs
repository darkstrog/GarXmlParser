using System.Xml;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class AddressObjectNodeMapper: IGarItemMapper<ADDRESSOBJECTSOBJECT>
    {
        public string NodeName => "OBJECT";

        public event Action<ADDRESSOBJECTSOBJECT> OnObjectMapped;
        public ADDRESSOBJECTSOBJECT GetFromXelement(XElement element)
        {
            var addressObject = new ADDRESSOBJECTSOBJECT()
            {
                ID = (int)element.Attribute("ID"),
                OBJECTID = (int)element.Attribute("OBJECTID"),
                OBJECTGUID = (string)element.Attribute("OBJECTGUID"),
                NAME = (string)element.Attribute("NAME"),
                CHANGEID = (long)element.Attribute("CHANGEID"),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (ADDRESSOBJECTSOBJECTISACTIVE)int.Parse((string)element.Attribute("ISACTIVE")),
                ISACTUAL = (ADDRESSOBJECTSOBJECTISACTUAL)int.Parse((string)element.Attribute("ISACTUAL")),
                LEVEL = (string)element.Attribute("LEVEL"),
                NEXTID = (long?)element.Attribute("NEXTID") ?? 0,
                NEXTIDSpecified = element.Attribute("NEXTID") != null,
                PREVID = (long?)element.Attribute("PREVID") ?? 0,
                PREVIDSpecified = (string)element.Attribute("PREVID") != null,
                OPERTYPEID = (string)element.Attribute("OPERTYPEID"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                TYPENAME = (string)element.Attribute("TYPENAME")
            };

            OnObjectMapped.Invoke(addressObject);

            return addressObject;
        }

        public static ADDRESSOBJECTSOBJECT GetAddressObjectFromReader(XmlReader reader)
        {
            var obj = new ADDRESSOBJECTSOBJECT();
            bool nextIdSpecified = false;
            bool prevIdSpecified = false;
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "ID":
                        obj.ID = int.Parse(reader.Value);
                        break;
                    case "OBJECTID":
                        obj.OBJECTID = int.Parse(reader.Value);
                        break;
                    case "OBJECTGUID":
                        obj.OBJECTGUID = reader.Value;
                        break;
                    case "NAME":
                        obj.NAME = reader.Value;
                        break;
                    case "CHANGEID":
                        obj.CHANGEID = long.Parse(reader.Value);
                        break;
                    case "UPDATEDATE":
                        obj.UPDATEDATE = DateTime.Parse(reader.Value);
                        break;
                    case "ENDDATE":
                        obj.ENDDATE = DateTime.Parse(reader.Value);
                        break;
                    case "ISACTIVE":
                        obj.ISACTIVE = (ADDRESSOBJECTSOBJECTISACTIVE)int.Parse(reader.Value);
                        break;
                    case "ISACTUAL":
                        obj.ISACTUAL = (ADDRESSOBJECTSOBJECTISACTUAL)int.Parse(reader.Value);
                        break;
                    case "LEVEL":
                        obj.LEVEL = reader.Value;
                        break;
                    case "NEXTID":
                        obj.NEXTID = long.Parse(reader.Value);
                        nextIdSpecified = true;
                        break;
                    case "PREVID":
                        obj.PREVID = long.Parse(reader.Value);
                        prevIdSpecified = true;
                        break;
                    case "OPERTYPEID":
                        obj.OPERTYPEID = reader.Value;
                        break;
                    case "STARTDATE":
                        obj.STARTDATE = DateTime.Parse(reader.Value);
                        break;
                    case "TYPENAME":
                        obj.TYPENAME = reader.Value;
                        break;
                    default:
                        break;
                }
            }

            reader.MoveToElement();

            obj.NEXTIDSpecified = nextIdSpecified;
            obj.PREVIDSpecified = prevIdSpecified;
            return obj;
        }

        private static void SetPropertyFromAttribute(string attrName, string attrValue, ADDRESSOBJECTSOBJECT obj)
        {
            switch (attrName)
            {
                case "ID":
                    obj.ID = int.Parse(attrValue);
                    break;
                case "OBJECTID":
                    obj.OBJECTID = int.Parse(attrValue);
                    break;
                case "OBJECTGUID":
                    obj.OBJECTGUID = attrValue;
                    break;
                case "NAME":
                    obj.NAME = attrValue;
                    break;
                case "CHANGEID":
                    obj.CHANGEID = long.Parse(attrValue);
                    break;
                case "UPDATEDATE":
                    obj.UPDATEDATE = DateTime.Parse(attrValue);
                    break;
                case "ENDDATE":
                    obj.ENDDATE = DateTime.Parse(attrValue);
                    break;
                case "ISACTIVE":
                    obj.ISACTIVE = (ADDRESSOBJECTSOBJECTISACTIVE)int.Parse(attrValue);
                    break;
                case "ISACTUAL":
                    obj.ISACTUAL = (ADDRESSOBJECTSOBJECTISACTUAL)int.Parse(attrValue);
                    break;
                case "LEVEL":
                    obj.LEVEL = attrValue;
                    break;
                case "NEXTID":
                    obj.NEXTID = long.Parse(attrValue);
                    break;
                case "NEXTIDSpecified":
                    obj.NEXTIDSpecified = attrValue != null;//есть вопросики
                    break;
                case "PREVID":
                    obj.PREVID = long.Parse(attrValue);//есть вопросики
                    break;
                case "PREVIDSpecified":
                    obj.PREVIDSpecified = attrValue != null;//есть вопросики
                    break;
                case "OPERTYPEID":
                    obj.OPERTYPEID = attrValue;
                    break;
                case "STARTDATE":
                    obj.STARTDATE = DateTime.Parse(attrValue);
                    break;
                case "TYPENAME":
                    obj.TYPENAME = attrValue;
                    break;
                default:
                    break;
            }
        }
    }
}

