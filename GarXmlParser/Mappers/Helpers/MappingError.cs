namespace GarXmlParser.Mappers.Helpers
{
    public class MappingError
    {
        public required Exception Exception { get; set; }
        public string FileName { get; set; } = "Без имени";
        public int LineNumber { get; set; }
        public string OriginalXmlElement { get; set; } = "Нет данных";
        public string AttributeName { get; set; } = "Нет данных";
        public DateTime ErrorTime { get; set; } = DateTime.UtcNow;
    }
}
