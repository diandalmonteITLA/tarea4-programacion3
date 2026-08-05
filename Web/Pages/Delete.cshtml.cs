using Application;
using Domain;
using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Listings
{
    public class DeleteModel : PageModel
    {
        private readonly ListingService _listingService;

        public DeleteModel(ListingService listingService)
        {
            _listingService = listingService;
        }

        [BindProperty]
        public Listing ListingToDelete { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            ListingToDelete = listing;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            await _listingService.DeactivateListingAsync(id);
            return RedirectToPage("./Index");
        }
    }
}