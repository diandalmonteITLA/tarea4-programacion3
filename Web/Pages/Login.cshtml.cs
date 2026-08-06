using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application.DTOs;

namespace Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AccountService _accountService;

        public LoginModel(AccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public LoginDto LoginInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _accountService.AuthenticateAsync(LoginInput);

            if (response.HasError)
            {
                ErrorMessage = response.Errors[0];
                return Page();
            }

            return RedirectToPage("/Index");
        }
    }
}
