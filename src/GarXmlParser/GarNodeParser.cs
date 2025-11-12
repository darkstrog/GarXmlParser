using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Linq;

namespace GarXmlParser
{
    /// <summary>
    /// Парсер пропускает ноду если маппер не смог ее обработать,
    /// обработка ошибки возложена на маппер
    /// </summary>
    internal class GarNodeParser
    {
    private readonly ILogger? _logger;

    public GarNodeParser(ILogger<GarNodeParser>? logger = null)
    {
        _logger = logger;
    }
        /// <summary>
        /// Получает объекты из XML асинхронно потоковым чтением по одному объекту из файла
        /// </summary>
        /// <typeparam name="T">Тип возвращаемый маппером</typeparam>
        /// <param name="filePath">Путь к XML файлу исходнику</param>
        /// <param name="mapper">Маппер с помощью которого будет осуществлена десериализация XElement в конечный класс</param>
        /// <param name="cancellationToken">Токен для отмены операции</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Если маппер или путь оказался null</exception>
        /// <exception cref="OperationCanceledException">Вызывается при отмене операции</exception>
        public async IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromFileAsync<T>(
            string filePath,
            IGarItemMapper<T> mapper,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
             where T : class
        {
            if (string.IsNullOrEmpty(filePath)) { throw new ArgumentNullException(filePath); }
            ArgumentNullException.ThrowIfNull(mapper);

            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                Async = true
            };

            using (var reader = XmlReader.Create(filePath, settings))
            {
                _logger?.LogInformation($"Начинается чтение: {filePath}");
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == mapper.NodeName)
                    {
                        using (var subtree = reader.ReadSubtree())
                        {
                            await subtree.ReadAsync().ConfigureAwait(false);
                            var lineNumber = ((IXmlLineInfo)reader).LineNumber;
                            var element = await XElement.LoadAsync(subtree, LoadOptions.None, cancellationToken).ConfigureAwait(false);
                            if (element != null)
                            {
                                var item = mapper.GetFromXelement(element, filePath, lineNumber);
                                if (item != null) yield return item;
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Получает объекты из XML асинхронно потоковым чтением по одному объекту из строки
        /// </summary>
        /// <typeparam name="T">Тип возвращаемый маппером</typeparam>
        /// <param name="xmlContent">Строка с XML содержимым</param>
        /// <param name="mapper">IGarItemMapper<T> Маппер с помощью которого будет осуществлена десериализация XElement в конечный класс</param>
        /// <param name="fileName">Имя XML контента, используется для логирования прогресса обработки</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Если маппер или путь оказался null</exception>
        /// <exception cref="OperationCanceledException">Вызывается при отмене операции</exception>
        public async IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromXMLContentAsync<T>
            (
                string xmlContent,
                IGarItemMapper<T> mapper,
                string fileName = "XML содержимое",
                [EnumeratorCancellation] CancellationToken cancellationToken = default
            ) where T : class
        {
            if (string.IsNullOrEmpty(xmlContent)) { throw new ArgumentNullException(xmlContent); }
            ArgumentNullException.ThrowIfNull(mapper);

            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                Async = true
            };

            using (var xmlString = new StringReader(xmlContent))
            {
                using (var reader = XmlReader.Create(xmlString, settings))
                {
                    _logger?.LogInformation($"Начинается чтение: {fileName}");
                    while (await reader.ReadAsync().ConfigureAwait(false))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (reader.NodeType == XmlNodeType.Element && reader.Name == mapper.NodeName)
                        {
                            using (var subtree = reader.ReadSubtree())
                            {
                                await subtree.ReadAsync().ConfigureAwait(false);
                                var lineNumber = ((IXmlLineInfo)reader).LineNumber;
                                var element = await XElement.LoadAsync(subtree, LoadOptions.None, cancellationToken).ConfigureAwait(false);
                                if (element != null)
                                {
                                    var item = mapper.GetFromXelement(element, fileName, lineNumber);
                                    if (item != null) yield return item;
                                }
                            }
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Обрабатывает XML из полученного стрима
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">Стрим с XML содержимым</param>
        /// <param name="fileName">Имя XML контента, используется для логирования прогресса обработки</param>
        /// <param name="mapper">IGarItemMapper<T> Маппер с помощью которого будет осуществлена десериализация XElement в конечный класс</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async IAsyncEnumerable<IMappedObject<T>> GetXmlObjectFromStreamAsync<T>(
           Stream stream,
           IGarItemMapper<T> mapper,
           string fileName="Содержимое потока",
           [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class
        {
            if (stream is null) { throw new ArgumentNullException("stream"); }
            ArgumentNullException.ThrowIfNull(mapper);

            var settings = new XmlReaderSettings
            {
                IgnoreComments = true,
                IgnoreWhitespace = true,
                Async = true
            };

            using (var reader = XmlReader.Create(stream, settings))
            {
                _logger?.LogInformation($"Начинается чтение: {fileName}");
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (reader.NodeType == XmlNodeType.Element && reader.Name == mapper.NodeName)
                    {
                        using (var subtree = reader.ReadSubtree())
                        {
                            await subtree.ReadAsync().ConfigureAwait(false);
                            var lineNumber = ((IXmlLineInfo)reader).LineNumber;
                            var element = await XElement.LoadAsync(subtree, LoadOptions.None, cancellationToken).ConfigureAwait(false);
                            if (element != null)
                            {
                                var item = mapper.GetFromXelement(element, fileName, lineNumber);
                                if (item != null) yield return item;
                            }
                        }
                    }
                }
            }
        }

    }
}
