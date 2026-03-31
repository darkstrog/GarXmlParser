namespace GARReplication.Core.Interfaces
{
    public interface IReplicationService
    {
        Task ReplicateAsync(string zipPath,
                            int batchSize,
                            IEnumerable<int>? regions,
                            string strategyName,
                            CancellationToken token);
    }
}
