using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class AddressObjectRepo : IAddressObjectRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM address_objects WHERE objectguid = @ObjectGuid LIMIT 1)";
        private const string GetByObjectIdAsyncQuery = "SELECT * FROM address_objects WHERE objectid = @ObjectId";
        private const string GetByIdAsyncQuery = "SELECT * FROM address_objects WHERE id = @Id";
        private const string GetActualByObjectIdAsyncQuery = "SELECT * FROM address_objects WHERE id = @Id AND isactual = true";
        private const string GetByGuidAsyncQuery = "SELECT * FROM address_objects WHERE objectguid = @ObjectGuid";
        private const string GetByLevelAndRegionAsyncQuery = "SELECT * FROM address_objects WHERE level = @Level and region = @Region";
        private const string CreateAsyncQuery = @"INSERT INTO address_objects (id, objectid, objectguid, changeid, name, typename,
                                                                               level, opertypeid, previd, nextid, updatedate, 
                                                                               startdate, enddate, isactual, isactive)
                                                  VALUES (@id, @objectid, @objectguid, @changeid, @name, @typename,
                                                                               @level, @opertypeid, @previd, @nextid, @updatedate, 
                                                                               @startdate, @enddate, @isactual, @isactive)"; 

        private const string CreateBulkAsyncQuery = @"INSERT INTO address_objects (id, objectid, objectguid, changeid, name, typename,
                                                                               level, opertypeid, previd, nextid, updatedate, 
                                                                               startdate, enddate, isactual, isactive)
                                                  VALUES (@id, @objectid, @objectguid, @changeid, @name, @typename,
                                                                               @level, @opertypeid, @previd, @nextid, @updatedate, 
                                                                               @startdate, @enddate, @isactual, @isactive)";
        private const string GetChildrenAsyncQuery = @"SELECT ao.* FROM address_objects ao 
                                                                  INNER JOIN adm_hierarchy ah ON ao.objectid = ah.objectid
                                                                  WHERE ah.parentobjectid = @parent_object_id  
                                                                    AND ah.isactive = 1
                                                                    AND ao.isactive = 1
                                                                  ORDER BY ao.name
                                                                  LIMIT @PageSize
                                                                 OFFSET (@Page - 1) * @PageSize";
        #endregion
        public AddressObjectRepo(string connectiobStringGar, ILogger<AddressObjectRepo> logger, string? connectionStringErrors = null)
        {
            _connectionStringGar = connectiobStringGar;
            _logger = logger;
        }
        public async Task InsertAsync(AddressObjectDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        /// <summary>
        /// Проверка записи на наличие в базе
        /// </summary>
        /// <param name="addressObject"></param>
        /// <returns></returns>
        public async Task<bool> IsExist(AddressObjectDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.OBJECTGUID });

                    if (exists)
                    {
                        _logger.LogInformation("Объект с GUID: {OBJECTGUID} уже существует в базе", entity.OBJECTGUID);
                        return true;
                    }

                    _logger.LogInformation("AddressObject с GUID: {OBJECTGUID} не найден", entity.OBJECTGUID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование AddressObject с GUID: {OBJECTGUID}", entity.OBJECTGUID);
                    throw;
                }
            }
        }

        /// <summary>
        /// Метод для массовой заливки AddressObjects в базу
        /// </summary>
        /// <param name="addressObjects"></param>
        /// <returns></returns>
        public async Task InsertBulkAsync(IEnumerable<AddressObjectDto> entities)
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
                    catch(Exception ex)
                    {
                        _logger?.LogError(ex, "Не удалось завершить транзакцию");
                        await createBulkTransaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }
        
        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressObjectDto>> GetActualByTypeAsync(string typeName, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressObjectDto>> GetByLevelAndRegionAsync(int level, string regionCode, int page, int pageSize)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Возвращает актуальный AddressObject по ObjectId
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async Task<AddressObjectDto?> GetActualByObjectIdAsync(long objectId)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                return await connection.QueryFirstOrDefaultAsync<AddressObjectDto>(
                GetActualByObjectIdAsyncQuery,
                new { ObjectId = objectId });
            }
        }
        /// <summary>
        /// Возвращает все AddressObject по полю ObjectId
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AddressObjectDto>> GetByObjectIdAsync(long objectId)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<AddressObjectDto>(
                    GetByObjectIdAsyncQuery, new { ObjectId = objectId });
            }
        }
        public async Task<AddressObjectDto?> GetByIdAsync(long id)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<AddressObjectDto?>(
                    GetByIdAsyncQuery, new { Id = id });
            }
        }
        /// <summary>
        /// Возвращает AddressObject по UUID
        /// </summary>
        /// <param name="objectGuid"></param>
        /// <returns></returns>
        public async Task<AddressObjectDto?> GetByGuidAsync(Guid objectGuid)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<AddressObjectDto?>(
                    GetByGuidAsyncQuery, new { ObjectGuid = objectGuid });
            }
        }

        public async Task<IEnumerable<AddressObjectDto>> GetChildrenAsync(long parentObjectId, int page, int pageSize)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();
                return await connection.QueryAsync<AddressObjectDto>(
                    GetChildrenAsyncQuery, new { parent_object_id = parentObjectId, Page = page, PageSize = pageSize});
            }
        }

        public Task<long> GetCountAsync(Expression<Func<AddressObjectDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressObjectDto>> GetPagedAsync(int page, int pageSize, Expression<Func<AddressObjectDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressObjectDto>> SearchByNameAsync(string name, int limit = 50)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AddressObjectDto entity)
        {
            throw new NotImplementedException();
        }
        
    }
}
