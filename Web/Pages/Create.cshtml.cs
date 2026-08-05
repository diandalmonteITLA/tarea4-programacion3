using Application;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Listings
{
    public class CreateModel : PageModel
    {
        private readonly ListingService _listingService;

        public CreateModel(ListingService listingService)
        {
            _listingService = listingService;
        }


        [BindProperty]
        public Listing NewListing { get; set; } = null!;


        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            await _listingService.CreateListingAsync(NewListing);


            return RedirectToPage("./Index");
        }
    }
}
