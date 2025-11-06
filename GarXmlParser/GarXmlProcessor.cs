using GarXmlParser.Mappers.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GarXmlParser
{
    public class GarXmlProcessor
    {
        private readonly GarNodeParser _parser;
        private readonly ILogger? _logger;

        public GarXmlProcessor(ILogger<GarXmlProcessor>? logger = null)
        {
            _parser = new GarNodeParser();
            _logger = logger;
            var cancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Принимает делегат Func<T, Task> для внедрения логики обработки десериализованного объекта.
        /// <example>
        ///        Пример вызова:
        ///<code>
        ///    List<string> filePaths = GetFiles();
        ///    var mapper = new HouseTypeNodeMapper();
        ///    CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        ///    CancellationToken token = cancelTokenSource.Token;
        /// 
        ///    await ProcessFilesAsync(
        ///    filePaths,
        ///    mapper,
        ///    async item =>{
        ///         await SaveToDatabaseAsync(item);
        ///         _logger.LogInformation($"Обработан объект: {item}")
        ///         ;},
        ///    token);
        ///        
        ///</code>
        ///</example>
        /// 
        /// Отмену операции логирует.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePaths">Список путей к файлам XML</param>
        /// <param name="mapper"></param>
        /// <param name="itemProcessor">Делегат для внедрения логики обработки</param>
        /// <param name="cancellationToken"></param>
        /// <param name="progress">Параметр для отслеживания прогресса выполнения парсинга</param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException">Вызывается при отмене операции</exception>
        public async Task<int> ProcessFilesAsync<T>(
            IEnumerable<string> filePaths,
            IGarItemMapper<T> mapper,
            Func<IMappedObject<T>, Task> itemProcessor,
            CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null) where T : class
        {
            var _files = filePaths.ToList();
            int _filesCount = _files.Count;
            int _totalCount = 0;
            int _fileIndex = 0;
            int _errorCount = 0;

            mapper.OnErrorMapping += error =>
            {
                _errorCount++;
                _logger?.LogDebug("Ошибка парсинга: {File}:{Line} - {Message}",
                    error.FileName, error.LineNumber, error.Exception.Message);
            };

            foreach (var file in _files)
            {
                _fileIndex++;
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await foreach (var item in _parser.GetXmlObjectFromFileAsync(file, mapper, cancellationToken))
                    {
                        await itemProcessor(item);
                        _totalCount++;
                        if (_totalCount % 100 == 0)
                        {
                            progress?.Report(new ProcessingProgress(_fileIndex, _filesCount, file, _totalCount, _errorCount));
                        }
                    }
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger?.LogError(ex, $"Ошибка обработки файла {file}");
                    throw;
                }
                catch (OperationCanceledException cancelex)
                {
                    _logger?.LogInformation($"Обработка отменена в вызывающем коде {cancelex}");
                    throw;
                }
            }
            
            if (_errorCount > 0)
            {
                _logger?.LogInformation("Обработан файлов {File}: {Success} успешно, {Errors} ошибок",
                    _fileIndex, _totalCount, _errorCount);
            }

            return _totalCount;
        }

        /// <summary>
        /// Обрабатывает список xml файлов
        /// Возвращает десериализованные объекты асинхронно
        /// Требует обработки исключения отмены операции в вызывающем коде
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePaths"></param>
        /// <param name="mapper"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// /// <exception cref="ArgumentNullException">Если маппер или путь оказался null</exception>
        /// <exception cref="OperationCanceledException">Вызывается при отмене операции</exception>
        public async IAsyncEnumerable<IMappedObject<T>> StreamFilesAsync<T>(
            IEnumerable<string> filePaths,
            IGarItemMapper<T> mapper,
            [EnumeratorCancellation] CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null) where T : class
        {
            int _totalCount = 0;
            int _fileIndex = 0;
            int _errorCount = 0;

            mapper.OnErrorMapping += error =>
            {
                _errorCount++;
                _logger?.LogDebug("Ошибка парсинга: {File}:{Line} - {Message}",
                    error.FileName, error.LineNumber, error.Exception.Message);
            };

            foreach (var filePath in filePaths)
            {
                _fileIndex++;
                await foreach (var item in _parser.GetXmlObjectFromFileAsync(filePath, mapper, cancellationToken))
                {
                    _totalCount++;
                    if (_totalCount % 100 == 0)
                    {
                        progress?.Report(new ProcessingProgress(_fileIndex, _fileIndex, filePath, _totalCount, _errorCount));
                    }
                    yield return item;
                }
            }

            if (_errorCount > 0)
            {
                _logger?.LogInformation("Обработан файлов {File}: {Success} успешно, {Errors} ошибок",
                    _fileIndex, _totalCount, _errorCount);
            }
        }

        /// <summary>
        /// Возвращает список десериализованных объектов
        /// Внимание! файлы читает потоком но есть затраты памяти на десериализованный список объектов
        /// В случае отмены обработки, возвращает список объектов полученных до отмены операции
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePaths">Пути к XML файлам</param>
        /// <param name="mapper">IGarItemMapper<T> Mapper для получаемой сущности</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Если маппер или путь оказался null</exception>
        /// <exception cref="OperationCanceledException">Вызывается при отмене операции</exception>
        public async Task<List<IMappedObject<T>>> GetListAsync<T>(
            IEnumerable<string> filePaths,
            IGarItemMapper<T> mapper,
            CancellationToken cancellationToken = default) where T : class
        {
            var results = new List<IMappedObject<T>>();
            try
            {
                await foreach (var item in StreamFilesAsync(filePaths, mapper, cancellationToken))
                {
                    results.Add(item);
                }

                return results;

            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger?.LogInformation($"Ошибка при обработке файла {ex.Message}");
                throw;
            }
            catch (OperationCanceledException ex)
            {
                _logger?.LogInformation($"Обработка остановлена в вызывающем коде{ex.Message}");
                return results;
            }

        }
        /// <summary>
        /// Возвращает десериализованные объекты асинхронно из файлов упакованных в zip архив
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="zipFilePath">Путь к zip архиву</param>
        /// <param name="regexPattern">Паттерн для поиска нужных XML в архиве</param>
        /// <param name="mapper">IGarItemMapper<T> Mapper для получаемой сущности</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <param name="progress">Параметр для отслеживания прогресса выполнения парсинга</param>
        /// <returns></returns>
        public async IAsyncEnumerable<IMappedObject<T>> StreamZipArchiveFilesAsync<T>(
            string zipFilePath,
            string regexPattern,
            IGarItemMapper<T> mapper,
            [EnumeratorCancellation] CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null
            ) where T : class
        {
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

            using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                var matchingEntries = archive.Entries
                    .Where(entry => !entry.FullName.EndsWith("/") && regex.IsMatch(entry.FullName))
                    .ToList();
                int _filesCount = matchingEntries.Count;

                int _totalItemsCount = 0;
                int _fileIndex = 0;
                int _errorCount = 0;

                foreach (ZipArchiveEntry entry in matchingEntries)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    string _fileName = entry.FullName;
                    
                    mapper.OnErrorMapping += error =>
                    {
                        _errorCount++;
                        _logger?.LogDebug("Ошибка парсинга: {File}:{Line} - {Message}",
                            error.FileName, error.LineNumber, error.Exception.Message);
                    };

                    using (Stream entryStream = entry.Open())
                    {
                        await foreach (var item in _parser.GetXmlObjectFromStreamAsync(entryStream, mapper, _fileName, cancellationToken))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            _totalItemsCount++;
                            if (_totalItemsCount % 100 == 0)
                            {
                                progress?.Report(new ProcessingProgress(_fileIndex, _filesCount, entry.Name, _totalItemsCount, _errorCount));
                            }
                            yield return item;
                        }
                    }
                    _fileIndex++;
                    _logger?.LogInformation("Завершена обработка: {FileName}", _fileName);
                    progress?.Report(new ProcessingProgress(_fileIndex, _filesCount, entry.Name, _totalItemsCount, _errorCount));
                    if (_errorCount > 0)
                    {
                        _logger?.LogInformation("Обработано файлов {File}: {Success} объектов получено успешно, {Errors} ошибок",
                            _fileIndex, _totalItemsCount, _errorCount);
                    }
                }
            }
        }
    }
}
