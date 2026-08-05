using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Listings
{
    public class EditModel : PageModel
    {
        private readonly ListingService _listingService;

        public EditModel(ListingService listingService)
        {
            _listingService = listingService;
        }

        [BindProperty]
        public Listing ListingToEdit { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var listing = await _listingService.GetListingByIdAsync(id);
            if (listing == null)
            {
                return NotFound();
            }

            ListingToEdit = listing;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _listingService.UpdateListingAsync(ListingToEdit);
            return RedirectToPage("./Index");
        }
    }
}