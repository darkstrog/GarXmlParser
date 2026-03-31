using GARReplication.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Xml.Linq;

namespace GARReplication.Core.Repository
{
    public class BulkRepo : IBulkRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<BulkRepo> _logger;
        public BulkRepo(ReplicationSettings configuration, ILogger<BulkRepo> logger)
        {
            _connectionString = configuration.ConnectionString;
            _logger = logger;
        }

        public async Task InsertBulkAsync(Queue<XElement> entities,
                                          IEntityWriter entityWriter,
                                          CancellationToken cancellationToken = default)
        {
            int entitiesCount = entities.Count;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var bulkTransaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        using (var binaryWriter = await connection.BeginBinaryImportAsync(entityWriter.InsertQuery))
                        {
                            int counter = 0;

                            while (entities.TryDequeue(out XElement? entity))
                            {
                                try
                                {
                                    if (++counter % 1000 == 0)
                                    {
                                        cancellationToken.ThrowIfCancellationRequested();
                                    }

                                    entityWriter.WriteRow(binaryWriter, entity);
                                }
                                catch (OperationCanceledException)
                                {
                                    _logger?.LogInformation("Импорт отменен, откат транзакции");
                                    await bulkTransaction.RollbackAsync(CancellationToken.None);
                                    throw;
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogCritical(ex, "Ошибка записи строки.");
                                    await bulkTransaction.RollbackAsync();
                                    throw;
                                }
                            }
                            await binaryWriter.CompleteAsync();
                        }
                        cancellationToken.ThrowIfCancellationRequested();
                        await bulkTransaction.CommitAsync();
                        _logger.LogInformation("Транзакция успешно завершена. Записано {entities} строк.", entitiesCount);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger?.LogInformation("Импорт отменен, откат транзакции");
                        await bulkTransaction.RollbackAsync(CancellationToken.None);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Не удалось завершить транзакцию");
                        await bulkTransaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
    }
}
