using GarXmlParser.GarEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class ObjectLevelNodeMapper : IGarItemMapper<ObjectLevel>
    {
        public string NodeName => "OBJECTLEVEL";

        public event Action<ObjectLevel>? OnObjectMapped;

        public ObjectLevel GetFromXelement(XElement element)
        {
            var objectLevel = new ObjectLevel()
            {
                LEVEL = (string)element.Attribute("LEVEL"),
                NAME = (string)element.Attribute("NAME"),
                SHORTNAME = (string)element.Attribute("SHORTNAME"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("ENDDATE")),
                ISACTIVE = (bool)element.Attribute("ISACTIVE")
            };

            OnObjectMapped?.Invoke(objectLevel);

            return objectLevel;
        }
    }
}
