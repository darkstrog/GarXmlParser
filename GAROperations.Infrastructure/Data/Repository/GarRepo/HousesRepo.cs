using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class HouseRepo: IHouseRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM houses WHERE id = @Id)";
        private const string GetByIdAsyncQuery = "SELECT * FROM houses WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO houses (id, objectid, objectguid, changeid, housenum, 
                                                                      addnum1, addnum2, housetype, addtype1, addtype2, 
                                                                      opertypeid, previd, nextid, updatedate, startdate, 
                                                                      enddate, isactual, isactive)
                                                  VALUES(@id, @objectid, @objectguid, @changeid, @housenum, 
                                                                      @addnum1, @addnum2, @housetype, @addtype1, @addtype2, 
                                                                      @opertypeid, @previd, @nextid, @updatedate, @startdate, 
                                                                      @enddate, @isactual, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO houses (id, objectid, objectguid, changeid, housenum, 
                                                                      addnum1, addnum2, housetype, addtype1, addtype2, 
                                                                      opertypeid, previd, nextid, updatedate, startdate, 
                                                                      enddate, isactual, isactive)
                                                  VALUES(@id, @objectid, @objectguid, @changeid, @housenum, 
                                                                      @addnum1, @addnum2, @housetype, @addtype1, @addtype2, 
                                                                      @opertypeid, @previd, @nextid, @updatedate, @startdate, 
                                                                      @enddate, @isactual, @isactive)";
        #endregion
        public HouseRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(HouseDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(HouseDto entity)
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

        public async Task InsertBulkAsync(IEnumerable<HouseDto> entities)
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

        public Task UpdateAsync(HouseDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HouseDto>> GetPagedAsync(int page, int pageSize, Expression<Func<HouseDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<HouseDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
