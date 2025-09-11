using GarXmlParser.GarEntities;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ParamTypeNodeMapper : IGarItemMapper<ParamType>
    {
        public string NodeName => "PARAMTYPE";

        public event Action<ParamType>? OnObjectMapped;

        public ParamType GetFromXelement(XElement element)
        {
            var paramType = new ParamType()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                CODE = (string)element.Attribute("CODE"),
                DESC = (string)element.Attribute("DESC"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ISACTIVE = (bool)element.Attribute("ISACTIVE")
            };

            OnObjectMapped?.Invoke(paramType);

            return paramType;
        }
    }
}
