using Entities.Dtos.Bitrix;
using Repository.Contracts;
using Services.Contracts.Bitrix;

namespace Services.Bitrix
{
    public class ContactService : IContactService
    {
        private readonly IBitrixRepositoryManager _repo;
        private readonly RedisCacheService _cache;

        public ContactService(IBitrixRepositoryManager repo, RedisCacheService cache)
        {
            _repo = repo;
            _cache = cache;
        }

        public async Task<ContactDto?> GetContact(int id)
        {
            var key = this.GetType().Name + "_Contact";
            var cachedData = await _cache.GetCachedData<ContactDto?>(key);

            if (cachedData is null)
            {
                var response = _repo.Contact.GetContact(id);
                var contact = (response is not null && response.HasResult) ? response.Result : null;
                await _cache.SetCacheData(key, contact, TimeSpan.FromSeconds(60));

                return contact;
            }

            return cachedData;
        }

        public async Task<IEnumerable<ContactDto>> GetContactsByFilter(string filter)
        {
            var key = this.GetType().Name + "_Contacts";
            var cachedData = await _cache.GetCachedData<IEnumerable<ContactDto>>(key);

            if (cachedData is null)
            {
                var response = _repo.Contact.GetContactsByFilter(filter);
                var contacts = (response is not null && response.IsSuccesful) ? response.Result : Enumerable.Empty<ContactDto>();
                await _cache.SetCacheData(key, contacts, TimeSpan.FromSeconds(60));

                return contacts!;
            }

            return cachedData;
        }
    }
}
