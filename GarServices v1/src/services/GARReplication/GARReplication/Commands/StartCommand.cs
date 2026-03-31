using GARReplication.Core;
using GARReplication.Core.EntityWriters;
using GARReplication.Core.Interfaces;
using GARReplication.Core.Repository;
using GARReplication.Core.Services;
using GARReplication.Settings;
using GarXmlParser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Diagnostics;

namespace GARReplication.Commands
{
    internal static class StartCommand
    {
        private static string? _cmdDataPath;
        private static int[]? _cmdRegions;
        private static int? _cmdBatchSize;
        private static string? _cmdStrategy;
        private static string? _cmdConnectionString;

        internal static Command Create()
        {
            var command = new Command("start", "Запускает процесс репликации.");

            var strategyOption = new Option<string>(name: "--strategy")
            {
                Description = "Стратегия репликации по умолчанию (fullrepl)",
                Required = false,
            };

            var pathOption = new Option<string>("--path", "-p")
            {
                Description = "Путь к XML файлу или дирректории",
                Required = false
            };

            var batchOption = new Option<int>("--batch", "-b")
            {
                Description = "Размер пачки для вставки в базу одной транзакцией",
                Required = false
            };

            var regionOption = new Option<int[]?>("--region", "-r")
            {
                Description = "Перечень кодов регионов для репликации. По умолчанию все регионы",
                AllowMultipleArgumentsPerToken = true,
                Required = false
            };

            var connectionStringOption = new Option<string>("--conns", "-cs")
            {
                Description = "Строка подключения к базе данных",
                Required = false
            };

            command.Add(strategyOption);
            command.Add(pathOption);
            command.Add(batchOption);
            command.Add(regionOption);
            command.Add(connectionStringOption);

            command.SetAction(async parseResult =>
            {
                _cmdDataPath = parseResult.GetValue<string>(pathOption);
                _cmdRegions = parseResult.GetValue<int[]?>(regionOption);
                _cmdBatchSize = parseResult.GetValue<int>(batchOption);
                _cmdStrategy = parseResult.GetValue<string>(strategyOption);
                _cmdConnectionString = parseResult.GetValue<string>(connectionStringOption);

                await RunReplicationAsync();
            });

            return command;
        }

        internal static ReplicationSettings MergeWithDefaults(ReplicationSettings defaultSettings)
        {
            return defaultSettings.WithCommandLineOverrides(
                dataPath: _cmdDataPath,
                regions: _cmdRegions,
                batchSize: _cmdBatchSize != 0 ? _cmdBatchSize : null,
                strategy: _cmdStrategy,
                connectionString: _cmdConnectionString
            );
        }

        private static async Task RunReplicationAsync()
        {
            var host = CreateHost();
            var replicationService = host.Services.GetRequiredService<IReplicationService>();
            var _replicationSettings = host.Services.GetRequiredService<ReplicationSettings>();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Процесс запущен");

            using var cancellationTokenSource = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, e) =>
            {
                logger.LogWarning("Отмена операции...");
                cancellationTokenSource.Cancel();
                e.Cancel = true;
            };
            Stopwatch operattionTime = new Stopwatch();
            operattionTime.Start();

            await replicationService.ReplicateAsync(
                        _replicationSettings.DataPath,
                        _replicationSettings.BatchSize,
                        _replicationSettings.Regions,
                        _replicationSettings.Strategy,
                        cancellationTokenSource.Token);

            operattionTime.Stop();
            var timeSpan = TimeSpan.FromMilliseconds(operattionTime.ElapsedMilliseconds);
            Console.WriteLine($"Время обработки: {timeSpan.Hours}ч : {timeSpan.Minutes}м : {timeSpan.Seconds}с : {timeSpan.Milliseconds}мс");
        }

        private static IHost CreateHost() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices)
            .Build();

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var defaultSettings = ReplicationSettingsFactory.FromConfiguration(config);
                return MergeWithDefaults(defaultSettings);
            });

            services.AddScoped<IGarNodeParser, GarNodeParser>();
            services.AddScoped<IGarXmlProcessor, GarXmlProcessor>();

            services.AddScoped<IWriterSelectionStrategy, FullReplStrategy>();
            services.AddScoped<IWriterSelectionStrategy, WithoutCHReplStrategy>();
            services.AddScoped<IWriterSelectionStrategy, SearchServiceStrategy>();
            
            services.AddScoped<IBulkRepository, BulkRepo>();
            services.AddScoped<IReplicationService, ReplicationService>();


            // Регистрируем Writer'ы для каждой сущности
            #region Регистрация EntityWriters

            services.AddTransient<AddressObjectParamsWriter>();
            services.AddTransient<ApartmentsParamsWriter>();
            services.AddTransient<CarPlaceParamsWriter>();
            services.AddTransient<HousesParamsWriter>();
            services.AddTransient<RoomsParamsWriter>();
            services.AddTransient<SteadsParamsWriter>();

            services.AddTransient<AddressObjectDivisionWriter>();
            services.AddTransient<AddressObjectTypeWriter>();
            services.AddTransient<AddressObjectWriter>();
            services.AddTransient<AdmHierarchyWriter>();
            services.AddTransient<ApartmentTypeWriter>();
            services.AddTransient<ApartmentWriter>();
            services.AddTransient<CarPlaceWriter>();
            services.AddTransient<ChangeHistoryWriter>();
            services.AddTransient<HouseTypeWriter>();
            services.AddTransient<HouseWriter>();
            services.AddTransient<MunHierarchyWriter>();
            services.AddTransient<NormativeDocKindWriter>();
            services.AddTransient<NormativeDocTypeWriter>();
            services.AddTransient<NormativeDocWriter>();
            services.AddTransient<ObjectLevelWriter>();
            services.AddTransient<OperationTypeWriter>();
            services.AddTransient<ParamTypeWriter>();
            services.AddTransient<ReestrObjectWriter>();
            services.AddTransient<RoomTypeWriter>();
            services.AddTransient<RoomWriter>();
            services.AddTransient<SteadWriter>();

            #endregion

            services.AddLogging(logging =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();
            });
        }
    }
}
