using GarXmlParser.Mappers.Helpers;
using System.Xml.Linq;

namespace GarXmlParser.Mappers.Interfaces
{
    /// <summary>
    /// NodeName - имя ноды ассоциируемой с классом T в xml.
    /// Событие OnObjectMapped срабатывающее по завершению наполнения экземпляра класса T.
    /// Событие OnErrorMapping срабатывает при исключениях при попытке маппинга
    /// GetFromXelement допускает возврат null в этом случае парсер пропустит элемент и продолжит читать дальше
    /// </summary>
    /// <typeparam name="T">Класс в который будет осуществлен маппинг</typeparam>
    public interface IGarItemMapper<T> where T : class
    {
        string NodeName { get; }
        event Action<IMappedObject<T>>? OnObjectMapped;
        event Action<MappingError>? OnErrorMapping;
        IMappedObject<T>? GetFromXelement(XElement element, string fileName, int lineNumber);
    }
}
