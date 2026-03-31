using GAROperations.Core.Models.GarModels;
using GarXmlParser.GarEntities;

namespace GAROperations.Core.Interfaces.Repository
{
    public interface IAddressObjectDivisionItemRepo: IGarRepository<AddressObjectDivisionItemDto>
    {
        Task<AddressObjectDivisionItemDto?> GetByIdAsync(long objectId);
    }
}
