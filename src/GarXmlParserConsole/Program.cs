using GarXmlParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace GarXmlParserConsole
{
    internal class Program
    {
        /// Пример парсинга и заливки в базу разделен на общие приготовления в main,
        /// и индивидуальные для каждой сущности, они находятся внутри класса ParseAndPushToBase
        /// так сделано что бы не нагромождать main
        /// В классе ParseAndPushToBase содержатся методы для каждой сущности
        /// в которых происходит вызов всех необходимых методов для парсинга сущности,
        /// ее маппинга и укладки в базу данных
        /// в методы ParseAndPushToBase передаются общие для всех сущностей экземпляры классов,
        /// такие как GarXmlProcessor - это Xml парсер для гар,
        /// zipPath - путь к архиву так как в данных примерах парсинг осуществляется из архива ГАР
        /// loggerFactory - внутри методов создаются логгеры для парсера и сервисов для работы с бд
        /// connectionString - строку подключения так же передаем в метод при вызове
        /// Остальные данные необходимые приготовления для парсинга сущности скрыты непосредственно в самих методах ParseAndPushToBase
        /// так как они индивидуальны для каждой сущности
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings_default.json", optional: true)
                                    .AddJsonFile("appsettings.json", optional: true)
                                    .AddEnvironmentVariables()
                                    .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                  ?? throw new InvalidOperationException("Отсутствует строка подключения");
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            string? zipPath = configuration.GetSection("SourceSettings:ZipSourcePath").Value;
            if (String.IsNullOrEmpty(zipPath)||!File.Exists(zipPath)) zipPath = InputSourcePath();
            var garProcessor = new GarXmlProcessor();

            var time_processing = new Stopwatch();
            time_processing.Start();

            /// в regions можно задать номера регионов 01-99 для парсинга отдельных регионов
            /// или не передавать regions тогда парсинг будет осущекствлен по всем регионам
            List<int>? regions = configuration.GetSection("RegionsSettings:Default").Get<List<int>>();

            ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
            });

            ParseAndPushToBase parseAndPushToBase = new ParseAndPushToBase();

            await parseAndPushToBase.AddressObjectDivisionDataProcess(garProcessor,
                                                                loggerFactory,
                                                                connectionString,
                                                                token,
                                                                zipPath, regions);
            await parseAndPushToBase.AddressObjectDataProcess(garProcessor,
                                                                loggerFactory,
                                                                connectionString,
                                                                token,
                                                                zipPath, regions);
            await parseAndPushToBase.AdmHierarchyDataProcess(garProcessor,
                                                                loggerFactory,
                                                                connectionString,
                                                                token,
                                                                zipPath, regions);
            time_processing.Stop();
            var timeSpan = TimeSpan.FromMilliseconds(time_processing.ElapsedMilliseconds);
            Console.WriteLine($"метод закончил обработку за {timeSpan.Minutes}м : {timeSpan.Seconds}с : {timeSpan.Milliseconds}мс");
            Console.WriteLine("Готово");
            Console.ReadLine();
        }
        static string InputSourcePath() 
        {
            string garPath;

            do
            {
                Console.WriteLine("Введите путь к XML источнику или q для выхода");
                garPath = Console.ReadLine() ?? "";

                if (garPath.ToLower() == "q") Environment.Exit(0);
            }
            while (!File.Exists(garPath));

            return garPath;
        }
    }
}
