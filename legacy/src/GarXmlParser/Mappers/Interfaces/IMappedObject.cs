namespace GarXmlParser.Mappers.Interfaces
{
    public interface IMappedObject<T>
    {
        public T Entity { get; set; }
        public string OriginalXmlElement { get; set; }
        public string SourceFilePath { get; set; }
        public long LineNumber { get; set; }
    }
}
