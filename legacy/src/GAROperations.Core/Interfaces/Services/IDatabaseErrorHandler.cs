namespace GAROperations.Core.Interfaces.Services
{
    public interface IDatabaseErrorHandler
    {
        bool IsTransientError(Exception exception);
        Exception InsertErrorHandle(Exception exception, string tableName);
        Exception UpdateErrorHandle(Exception exception, string tableName);
        Exception ConnectionErrorHandle(Exception exception);
    }
}
