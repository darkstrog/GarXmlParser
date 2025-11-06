using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class NormativeDocRepo: INormativeDocRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM normative_doc WHERE id = @Id)";
        private const string GetByIdAsyncQuery = "SELECT * FROM normative_doc WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO normative_doc (id, name, doc_date, doc_number, type, kind, 
                                                                             update_date, org_name, reg_num, reg_date, 
                                                                             acc_date, comment)
                                                  VALUES(@id, @name, @doc_date, @doc_number, @type, @kind, 
                                                                             @update_date, @org_name, @reg_num, @reg_date, 
                                                                             @acc_date, @comment)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO normative_doc (id, name, doc_date, doc_number, type, kind, 
                                                                             update_date, org_name, reg_num, reg_date, 
                                                                             acc_date, comment)
                                                  VALUES(@id, @name, @doc_date, @doc_number, @type, @kind, 
                                                                             @update_date, @org_name, @reg_num, @reg_date, 
                                                                             @acc_date, @comment)";
        #endregion
        public NormativeDocRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(NormativeDocDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(NormativeDocDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID });

                    if (exists)
                    {
                        _logger.LogInformation("Объект с ID: {ID} уже существует в базе", entity.ID);
                        return true;
                    }

                    _logger.LogInformation("Объект с ID: {ID} не найден", entity.ID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование ID: {ID}", entity.ID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<NormativeDocDto> entities)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();

                using (var createBulkTransaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await connection.ExecuteAsync(CreateBulkAsyncQuery,
                                                      entities,
                                                      transaction: createBulkTransaction
                                                      );
                        await createBulkTransaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Не удалось завершить транзакцию");
                        await createBulkTransaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public Task UpdateAsync(NormativeDocDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<NormativeDocDto>> GetPagedAsync(int page, int pageSize, Expression<Func<NormativeDocDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<NormativeDocDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
