using Entities.Dtos.Bitrix;

namespace Services.Contracts.Bitrix
{
    public interface IContactService
    {
        Task<ContactDto?> GetContact(int id);
        Task<IEnumerable<ContactDto>> GetContactsByFilter(string filter);
    }
}
