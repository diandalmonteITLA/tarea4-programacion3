using System.Reflection;
using Domain;
using Domain.Interfaces;

namespace Application
{
    public class ListingService
    {
        private readonly IGenericRepository<Listing> _repository;

        public ListingService(IGenericRepository<Listing> repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyCollection<Listing>> GetAllListingsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Listing?> GetListingByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task CreateListingAsync(Listing listing)
        {
            await _repository.AddAsync(listing);
        }

        public async Task UpdateListingAsync(Listing listing)
        {
            await _repository.UpdateAsync(listing);
        }

        public async Task DeactivateListingAsync(Guid id)
        {
            await _repository.DeactiveAsync(id);
        }
    }
}
