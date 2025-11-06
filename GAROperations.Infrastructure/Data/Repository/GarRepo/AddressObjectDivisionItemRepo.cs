using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class AddressObjectDivisionItemRepo : IAddressObjectDivisionItemRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM address_object_division WHERE id = @Id LIMIT 1)";
        private const string GetByIdAsyncQuery = "SELECT * FROM address_object_division WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO address_object_division (id, parentid, childid, changeid)
                                                  VALUES (@id, @parentid, @childid, @changeid)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO address_object_division (id, parentid, childid, changeid)
                                                    VALUES (@id, @parentid, @childid, @changeid)";
        #endregion
        public AddressObjectDivisionItemRepo(string connectiobStringGar, ILogger<AddressObjectDivisionItemRepo> logger, string? connectionStringErrors = null)
        {
            _connectionStringGar = connectiobStringGar;
            _logger = logger;
        }

        public async Task<bool> IsExist(AddressObjectDivisionItemDto entity)
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

        public async Task InsertAsync(AddressObjectDivisionItemDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task InsertBulkAsync(IEnumerable<AddressObjectDivisionItemDto> entities)
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

        public async Task<AddressObjectDivisionItemDto?> GetByIdAsync(long objectId)
        {
            using (var connection = new NpgsqlConnection(_connectionStringGar))
            {
                await connection.OpenAsync();
                return await connection.QueryFirstOrDefaultAsync<AddressObjectDivisionItemDto?>(
                    GetByIdAsyncQuery, new { Id = objectId });
            }
        }

        public Task UpdateAsync(AddressObjectDivisionItemDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AddressObjectDivisionItemDto>> GetPagedAsync(int page, int pageSize, Expression<Func<AddressObjectDivisionItemDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<AddressObjectDivisionItemDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
