using GarXmlParser.GarEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    public class OperationTypeNodeMapper : IGarItemMapper<OperationType>
    {
        public string NodeName => "OPERATIONTYPE";
        public event Action<OperationType>? OnObjectMapped;

        public OperationType GetFromXelement(XElement element)
        {
            var operationType = new OperationType()
            {
                ID = (string)element.Attribute("ID"),
                NAME = (string)element.Attribute("NAME"),
                SHORTNAME = (string)element.Attribute("SHORTNAME"),
                DESC = (string)element.Attribute("DESC"),
                STARTDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                UPDATEDATE = DateTime.Parse((string)element.Attribute("UPDATEDATE")),
                ENDDATE = DateTime.Parse((string)element.Attribute("STARTDATE")),
                ISACTIVE = (bool)element.Attribute("ISACTIVE")
            };

            OnObjectMapped?.Invoke(operationType);

            return operationType;
        }
    }
}
