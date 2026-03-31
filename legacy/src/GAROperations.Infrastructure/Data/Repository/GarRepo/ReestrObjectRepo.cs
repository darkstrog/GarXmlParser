using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class ReestrObjectRepo: IReestrObjectRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM reestr_object WHERE objectid = @Objectid AND changeid = @Changeid)";
        private const string GetByIdAsyncQuery = @"SELECT objectid, createdate, changeid, levelid, updatedate, objectguid, isactive 
                                                   FROM reestr_object
                                                   WHERE objectid = @Objectid AND changeid = @Changeid";
        private const string CreateAsyncQuery = @"INSERT INTO reestr_object (objectid, createdate, changeid, 
                                                                             levelid, updatedate, objectguid, isactive)
                                                  VALUES(@objectid, @createdate, @changeid, @levelid, @updatedate, @objectguid, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO reestr_object(objectid, createdate, changeid, levelid,
                                                                                updatedate, objectguid, isactive)
                                                      VALUES(@objectid, @createdate, @changeid, @levelid,
                                                             @updatedate, @objectguid, @isactive)";
        #endregion
        public ReestrObjectRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(ReestrObjectDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(ReestrObjectDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.OBJECTID, entity.CHANGEID });

                    if (exists)
                    {
                        _logger.LogInformation("Объект с ObjectId: {OBJECTID} и ChangeId: {CHANGEID} уже существует в базе", entity.OBJECTID, entity.CHANGEID);
                        return true;
                    }

                    _logger.LogInformation("Объект с с ObjectId: {OBJECTID} и ChangeId: {CHANGEID} не найден", entity.OBJECTID, entity.CHANGEID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование с ObjectId: {OBJECTID} и ChangeId: {CHANGEID}", entity.OBJECTID, entity.CHANGEID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<ReestrObjectDto> entities)
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

        public Task UpdateAsync(ReestrObjectDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ReestrObjectDto>> GetPagedAsync(int page, int pageSize, Expression<Func<ReestrObjectDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<ReestrObjectDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
