using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class RoomTypeRepo: IRoomTypeRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM room_type WHERE id = @Id)";
        private const string GetByIdAsyncQuery = @"SELECT id, name, shortname, description, updatedate, startdate, enddate, isactive 
                                                   FROM room_type
                                                   WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO room_type (id, name, shortname, description, updatedate,
                                                                         startdate, enddate, isactive)
                                                  VALUES(@id, @name, @shortname, @description, @updatedate,
                                                         @startdate, @enddate, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO room_type (id, name, shortname, description, updatedate,
                                                                         startdate, enddate, isactive)
                                                  VALUES(@id, @name, @shortname, @description, @updatedate,
                                                         @startdate, @enddate, @isactive)";
        #endregion
        public RoomTypeRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(RoomTypeDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(RoomTypeDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID });

                    if (exists)
                    {
                        _logger.LogInformation("RoomType с Id: {ID}: уже существует в базе", entity.ID);
                        return true;
                    }

                    _logger.LogInformation("RoomType с Id: {ID}: не найден", entity.ID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование RoomType с Id: {ID}", entity.ID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<RoomTypeDto> entities)
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

        public Task UpdateAsync(RoomTypeDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RoomTypeDto>> GetPagedAsync(int page, int pageSize, Expression<Func<RoomTypeDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<RoomTypeDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
