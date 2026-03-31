using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;


namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
                 
    public class AdmHierarchyRepo : IAdmHierarchyRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM adm_hierarchy WHERE id = @Id LIMIT 1)";
        private const string GetByIdAsyncQuery = @"SELECT id, objectid, parentobjid, changeid, regioncode, areacode, 
                                                                citycode, placecode, plancode, streetcode, previd, 
                                                                nextid, updatedate, startdate, enddate, isactive, ""path""
                                                   FROM public.adm_hierarchy WHERE id = @Id";

        private const string CreateAsyncQuery = @"INSERT INTO public.adm_hierarchy
                                                                (id, objectid, parentobjid, changeid, regioncode,
                                                                areacode, citycode, placecode, plancode, streetcode, 
                                                                previd, nextid, updatedate, startdate, enddate, isactive, ""path"")
                                                      VALUES (@id, @objectid, @parentobjid, @changeid, @regioncode,
                                                                @areacode, @citycode, @placecode, @plancode, @streetcode, 
                                                                @previd, @nextid, @updatedate, @startdate, @enddate, @isactive, @path)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO public.adm_hierarchy
                                                                (id, objectid, parentobjid, changeid, regioncode,
                                                                areacode, citycode, placecode, plancode, streetcode, 
                                                                previd, nextid, updatedate, startdate, enddate, isactive, ""path"")
                                                      VALUES (@id, @objectid, @parentobjid, @changeid, @regioncode,
                                                                @areacode, @citycode, @placecode, @plancode, @streetcode, 
                                                                @previd, @nextid, @updatedate, @startdate, @enddate, @isactive, @path)";
        #endregion
        public AdmHierarchyRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(AdmHierarchyDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task InsertBulkAsync(IEnumerable<AdmHierarchyDto> entities)
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
                        _logger?.LogError($"Не удалось завершить транзакцию по причине{ex.Message}");
                        await createBulkTransaction.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<bool> IsExist(AdmHierarchyDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.ID });

                    if (exists)
                    {
                        _logger.LogInformation($"Объект с ID: {entity.ID} уже существует в базе");
                        return true;
                    }

                    _logger.LogInformation($"AddressObject с ID: {entity.ID} не найден");
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Не удалась проверить существование AddressObject с ID: {entity.ID}");
                    throw;
                }
            }
        }

        public Task UpdateAsync(AdmHierarchyDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AdmHierarchyDto>> GetPagedAsync(int page, int pageSize, Expression<Func<AdmHierarchyDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<AdmHierarchyDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
