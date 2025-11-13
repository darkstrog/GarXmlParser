using Dapper;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class SteadRepo: ISteadRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM stead WHERE id = @Id)";
        private const string GetByIdAsyncQuery = @"SELECT id, objectid, objectguid, changeid, steadnumber,
                                                          opertypeid, previd, nextid, updatedate, startdate,
                                                          enddate, isactual, isactive
                                                   FROM stead
                                                   WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO stead (id, objectid, objectguid, changeid, steadnumber,
                                                                     opertypeid, previd, nextid, updatedate, startdate,
                                                                     enddate, isactual, isactive)
                                                  VALUES(@id, @objectid, @objectguid, @changeid, @steadnumber,
                                                                     @opertypeid, @previd, @nextid, @updatedate, @startdate,
                                                                     @enddate, @isactual, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO stead (id, objectid, objectguid, changeid, steadnumber,
                                                                     opertypeid, previd, nextid, updatedate, startdate,
                                                                     enddate, isactual, isactive)
                                                  VALUES(@id, @objectid, @objectguid, @changeid, @steadnumber,
                                                                     @opertypeid, @previd, @nextid, @updatedate, @startdate,
                                                                     @enddate, @isactual, @isactive)";
        #endregion
        public SteadRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(SteadDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(SteadDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID });

                    if (exists)
                    {
                        _logger.LogInformation("Stead с Id: {ID}: уже существует в базе", entity.ID);
                        return true;
                    }

                    _logger.LogInformation("Stead с Id: {ID}: не найден", entity.ID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование Stead с Id: {ID}", entity.ID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<SteadDto> entities)
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

        public Task UpdateAsync(SteadDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SteadDto>> GetPagedAsync(int page, int pageSize, Expression<Func<SteadDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<SteadDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
