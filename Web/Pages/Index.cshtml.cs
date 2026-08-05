using Application;
using Domain;
using Application;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Listings
{
    public class IndexModel : PageModel
    {
        private readonly ListingService _listingService;

        public IndexModel(ListingService listingService)
        {
            _listingService = listingService;
        }

        public IReadOnlyCollection<Listing> Listings { get; set; } = new List<Listing>();

        public async Task OnGetAsync()
        {
            Listings = await _listingService.GetAllListingsAsync();
        }
    }
}
