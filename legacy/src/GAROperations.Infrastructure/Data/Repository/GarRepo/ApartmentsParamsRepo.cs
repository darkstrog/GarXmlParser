using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class ApartmentsParamsRepo: IApartmentsParamsRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM apartments_params WHERE id = @Id)";
        private const string GetByIdAsyncQuery = @"SELECT id, objectid, changeid, changeidend, typeid,
                                                          value, updatedate, startdate, enddate
                                                   FROM apartments_params
                                                   WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO apartments_params (id, objectid, changeid, changeidend, typeid,
                                                                               value, updatedate, startdate, enddate)
                                                  VALUES(@id, @objectid, @changeid, @changeidend, @typeid,
                                                                               @value, @updatedate, @startdate, @enddate)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO apartments_params (id, objectid, changeid, changeidend, typeid,
                                                                               value, updatedate, startdate, enddate)
                                                  VALUES(@id, @objectid, @changeid, @changeidend, @typeid,
                                                                               @value, @updatedate, @startdate, @enddate)";
        #endregion
        public ApartmentsParamsRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(ParamDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(ParamDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID });

                    if (exists)
                    {
                        _logger.LogInformation("AddrObjParam с Id: {ID}: уже существует в базе", entity.ID);
                        return true;
                    }

                    _logger.LogInformation("AddrObjParam с Id: {ID}: не найден", entity.ID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование AddrObjParam с Id: {ID}", entity.ID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<ParamDto> entities)
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

        public Task UpdateAsync(ParamDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ParamDto>> GetPagedAsync(int page, int pageSize, Expression<Func<ParamDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<ParamDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
