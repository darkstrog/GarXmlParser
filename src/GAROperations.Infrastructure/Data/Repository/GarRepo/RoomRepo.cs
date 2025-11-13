using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class RoomRepo : IRoomRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM room WHERE id = @Id)";
        private const string GetByIdAsyncQuery = @"SELECT objectid, createdate, changeid, levelid, updatedate, objectguid, isactive 
                                                   FROM room
                                                   WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO room (id, objectid, objectguid, changeid, roomnumber, roomtype,
                                                                    opertypeid, previd, nextid, updatedate, startdate, enddate,
                                                                    isactual, isactive)
                                                  VALUES(@id, @objectid, @objectguid, @changeid, @roomnumber, @roomtype,
                                                         @opertypeid, @previd, @nextid, @updatedate, @startdate, @enddate,
                                                         @isactual, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO room (id, objectid, objectguid, changeid, roomnumber, roomtype,
                                                                        opertypeid, previd, nextid, updatedate, startdate, enddate,
                                                                        isactual, isactive)
                                                      VALUES(@id, @objectid, @objectguid, @changeid, @roomnumber, @roomtype,
                                                             @opertypeid, @previd, @nextid, @updatedate, @startdate, @enddate,
                                                             @isactual, @isactive)";
        #endregion
        public RoomRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(RoomDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(RoomDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID});

                    if (exists)
                    {
                        _logger.LogInformation("Room с Id: {ID}: уже существует в базе", entity.ID);
                        return true;
                    }

                    _logger.LogInformation("Room с Id: {ID}: не найден", entity.ID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование Room с Id: {ID}", entity.ID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<RoomDto> entities)
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

        public Task UpdateAsync(RoomDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoomDto>> GetPagedAsync(int page, int pageSize, Expression<Func<RoomDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<RoomDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
