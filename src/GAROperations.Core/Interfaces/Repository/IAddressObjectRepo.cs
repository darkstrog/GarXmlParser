using GAROperations.Core.Models.GarModels;

namespace GAROperations.Core.Interfaces.Repository
{
    public interface IAddressObjectRepo:IGarRepository<AddressObjectDto>
    {
        Task<IEnumerable<AddressObjectDto>> SearchByNameAsync(string name, int limit = 50);
        Task<IEnumerable<AddressObjectDto>> GetByObjectIdAsync(long objectId);
        Task<AddressObjectDto?> GetByIdAsync(long objectId);
        Task<IEnumerable<AddressObjectDto>> GetByLevelAndRegionAsync(int level, string regionCode, int page, int pageSize);
        Task<IEnumerable<AddressObjectDto>> GetChildrenAsync(long parentObjectId, int page, int pageSize);
        Task<IEnumerable<AddressObjectDto>> GetActualByTypeAsync(string typeName, int page, int pageSize);
    }
}
