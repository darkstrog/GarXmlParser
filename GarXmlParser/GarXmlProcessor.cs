using GarXmlParser.Mappers;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace GarXmlParser
{
    public class GarXmlProcessor
    {
        private readonly GarNodeParser _parser;
        private readonly ILogger _logger;

        public GarXmlProcessor(ILogger<GarXmlProcessor> logger = null)
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
            Func<T, Task> itemProcessor,
            CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null)
        {
            var files = filePaths.ToList();
            int filesCount = files.Count;
            int totalCount = 0;
            int fileIndex = 0;


            foreach (var file in files)
            {
                fileIndex++;
                progress?.Report(new ProcessingProgress(fileIndex, filesCount, file, totalCount));
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    await foreach (var item in _parser.GetXmlObjectFromFileAsync(file, mapper, cancellationToken))
                    {
                        await itemProcessor(item);
                        totalCount++;
                        progress?.Report(new ProcessingProgress(fileIndex, filesCount, file, totalCount));
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

            return totalCount;
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
        public async IAsyncEnumerable<T> StreamFilesAsync<T>(
            IEnumerable<string> filePaths,
            IGarItemMapper<T> mapper,
            [EnumeratorCancellation] CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null)
        {
            int totalCount = 0;
            int fileIndex = 0;
            foreach (var filePath in filePaths)
            {
                fileIndex++;
                await foreach (var item in _parser.GetXmlObjectFromFileAsync(filePath, mapper, cancellationToken))
                {
                    totalCount++;
                    yield return item;
                }
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
        public async Task<List<T>> GetListAsync<T>(
            IEnumerable<string> filePaths,
            IGarItemMapper<T> mapper,
            CancellationToken cancellationToken = default)
        {
            var results = new List<T>();
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
                _logger?.LogInformation("Обработка остановлена в вызывающем коде");
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
        public async IAsyncEnumerable<T> StreamZipArchiveFilesAsync<T>(
            string zipFilePath,
            string regexPattern,
            IGarItemMapper<T> mapper,
            [EnumeratorCancellation] CancellationToken cancellationToken = default,
            IProgress<ProcessingProgress>? progress = null
            )
        {
            var regex = new Regex(regexPattern, RegexOptions.IgnoreCase);

            using (FileStream zipStream = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, true))
            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Read))
            {
                var matchingEntries = archive.Entries
                    .Where(entry => !entry.FullName.EndsWith("/") && regex.IsMatch(entry.Name))
                    .ToList();
                int filesCount = matchingEntries.Count;
                foreach (ZipArchiveEntry entry in matchingEntries)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    int totalItemsCount = 0;
                    int fileIndex = 0;
                    string fileName = entry.FullName;

                    using (Stream entryStream = entry.Open())
                    {
                        await foreach (var item in _parser.GetXmlObjectFromStreamAsync(entryStream, mapper, fileName, cancellationToken))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            totalItemsCount++;
                            progress?.Report(new ProcessingProgress(fileIndex, filesCount, entry.Name, totalItemsCount));
                            yield return item;
                        }
                    }
                    fileIndex++;
                    _logger?.LogInformation("Завершена обработка: {FileName}", fileName);
                }
            }
        }
    }
}
