using GarXmlParser.Mappers.Interfaces;

namespace GarXmlParser.Mappers.Helpers
{
    /// <summary>
    ///  Контейнер для сущности с информацией для отладки
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappedObject<T> : IMappedObject<T>
    {
        public required T Entity { get; set; }
        public string OriginalXmlElement { get; set; } = "Нет данных";
        public string SourceFilePath { get; set; } = "Нет данных";
        public long LineNumber { get; set; }
    }
}
