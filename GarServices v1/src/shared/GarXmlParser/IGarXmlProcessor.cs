using System.Xml.Linq;

namespace GarXmlParser
{
    public interface IGarXmlProcessor
    {
        public IAsyncEnumerable<XElement> StreamZipArchiveFilesAsync(
            string zipFilePath,
            string regexPattern,
            string nodeName,
            CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null
            );
    }
}
