using System.Xml.Linq;

namespace GARReplication.Core.Interfaces
{
    public interface IBulkRepository
    {
        Task InsertBulkAsync(Queue<XElement> entities,
                                             IEntityWriter entityWriter,
                                             CancellationToken cancellationToken = default);
    }
}
