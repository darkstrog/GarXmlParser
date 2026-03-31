using Dapper;
using GAROperations.Core.Interfaces.Repository;
using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GAROperations.Infrastructure.Data.Repository.GarRepo
{
    public class ChangeHistoryItemRepo: IChangeHistoryRepo
    {
        private readonly string _connectionStringGar;
        private readonly ILogger _logger;

        #region sql-запросы
        private const string IsExistQuery = "SELECT EXISTS(SELECT 1 FROM change_history_item WHERE id = @Id LIMIT 1)";
        private const string GetByIdAsyncQuery = "SELECT * FROM change_history_item WHERE id = @Id";
        private const string CreateAsyncQuery = @"INSERT INTO change_history_item (id, name, shortname, description, 
                                                                              updatedate, startdate, enddate, isactive)
                                                  VALUES (@id, @name, @shortname, @description, @updatedate, @startdate, 
                                                                              @enddate, @isactive)";

        private const string CreateBulkAsyncQuery = @"INSERT INTO change_history_item (id, name, shortname, description, 
                                                                              updatedate, startdate, enddate, isactive)
                                                      VALUES (@id, @name, @shortname, @description, @updatedate, @startdate, 
                                                                              @enddate, @isactive)";
        #endregion
        public ChangeHistoryItemRepo(string connectionStringGar, ILogger logger)
        {
            _connectionStringGar = connectionStringGar;
            _logger = logger;
        }

        public async Task InsertAsync(ChangeHistoryItemDto entity)
        {
            using var db = new NpgsqlConnection(_connectionStringGar);
            await db.ExecuteAsync(CreateAsyncQuery, entity);
        }

        public async Task<bool> IsExist(ChangeHistoryItemDto entity)
        {
            using (var db = new NpgsqlConnection(_connectionStringGar))
            {
                try
                {
                    var exists = await db.ExecuteScalarAsync<bool>(IsExistQuery, new { entity.CHANGEID });

                    if (exists)
                    {
                        _logger.LogInformation("Объект с ID: {ID} уже существует в базе", entity.CHANGEID);
                        return true;
                    }

                    _logger.LogInformation("Объект с ID: {ID} не найден", entity.CHANGEID);
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалась проверить существование ID: {ID}", entity.CHANGEID);
                    throw;
                }
            }
        }

        public async Task InsertBulkAsync(IEnumerable<ChangeHistoryItemDto> entities)
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

        public Task UpdateAsync(ChangeHistoryItemDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long objectId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ChangeHistoryItemDto>> GetPagedAsync(int page, int pageSize, Expression<Func<ChangeHistoryItemDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<long> GetCountAsync(Expression<Func<ChangeHistoryItemDto, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
