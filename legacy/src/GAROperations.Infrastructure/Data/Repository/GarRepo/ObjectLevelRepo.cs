using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class ObjectLevelRepo: IObjectLevelRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM object_levels WHERE id = @Id)";
        private const string GetByIdAsyncQuery = "SELECT * FROM object_levels WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO object_levels (level, name, shortname, updatedate, startdate, enddate, isactive)
                                                  VALUES(@level, @name, @shortname, @updatedate, @startdate, @enddate, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO object_levels (level, name, shortname, updatedate, startdate, enddate, isactive)
                                                      VALUES(@level, @name, @shortname, @updatedate, @startdate, @enddate, @isactive)";
        #endregion
        public ObjectLevelRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(ObjectLevelDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(ObjectLevelDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.LEVEL});

                    if (exists)
                    {
                        _logger.LogInformation("Объект с ID: {ID} уже существует в базе", entity.LEVEL);
                        return true;
                    }

                    _logger.LogInformation("Объект с ID: {ID} не найден", entity.LEVEL);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование ID: {ID}", entity.LEVEL);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<ObjectLevelDto> entities)
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

        public Task UpdateAsync(ObjectLevelDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ObjectLevelDto>> GetPagedAsync(int page, int pageSize, Expression<Func<ObjectLevelDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<ObjectLevelDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
