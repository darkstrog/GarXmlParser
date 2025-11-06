namespace GAROperations.Core.Interfaces.Services
{
    public interface IDataStorageService<T> where T : class
    {
        Task InsertDataAsync(T data);
        Task InsertDataBulkAsync(IAsyncEnumerable<T> data, int batchSize = 1000);

    }
}
