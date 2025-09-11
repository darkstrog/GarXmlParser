using System.Xml.Linq;

namespace GarXmlParser.Mappers
{
    /// <summary>
    /// NodeName - имя ноды ассоциируемой с классом T в xml.
    /// Имеет необязательное событие OnObjectMapped срабатывающее по завершению наполнения экземпляра класса T.
    /// </summary>
    /// <typeparam name="T">Класс в который будет осуществлен маппинг</typeparam>
    public interface IGarItemMapper<T>
    {
        string NodeName { get; }
        event Action<T>? OnObjectMapped;
        T GetFromXelement(XElement element);
    }
}
