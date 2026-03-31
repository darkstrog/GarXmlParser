using GarXmlParser.Mappers.Interfaces;
using System.Xml.Linq;

namespace GarXmlParser
{
    //TODO: Необходимо добавить перегрузки для GetXmlObjectFromFileAsync и GetXmlObjectFromXMLContentAsync
    //возвращающие IAsyncEnumerable<Tuple<XElement, string, int>> по аналогии с GetXmlObjectFromStreamAsync
    
    public interface IGarNodeParser
    {
        public IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromFileAsync<T>(
                                                            string filePath,
                                                            IGarItemMapper<T> mapper,
                                                            CancellationToken cancellationToken = default)
                                                            where T : class;
        
        public IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromXMLContentAsync<T>(
                                                            string xmlContent,
                                                            IGarItemMapper<T> mapper,
                                                            string fileName = "XML содержимое",
                                                            CancellationToken cancellationToken = default)
                                                            where T : class;
        
        public IAsyncEnumerable<Tuple<XElement, string, int>> GetXmlObjectFromStreamAsync(
                                                            Stream stream,
                                                            string nodeName,
                                                            string fileName = "Содержимое потока",
                                                            CancellationToken cancellationToken = default);

        public IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromStreamAsync<T>(
                                                            Stream stream,
                                                            IGarItemMapper<T> mapper,
                                                            string fileName = "Содержимое потока",
                                                            CancellationToken cancellationToken = default) where T : class;
    }
}