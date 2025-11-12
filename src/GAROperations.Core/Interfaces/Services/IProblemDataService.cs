namespace GAROperations.Core.Interfaces.Services
{
    public interface IProblemDataService<T> where T : class
    {
        Task ProcessProblematicRecord(T record, string? error);
    }
}
