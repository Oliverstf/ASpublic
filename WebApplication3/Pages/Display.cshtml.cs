using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize]
    public class DisplayModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
		public DisplayModel(UserManager<ApplicationUser> userManager)
		{
			this.userManager = userManager;
		}
		[BindProperty]
		public ApplicationUser UserDis { get; set; }
		public async Task<IActionResult> OnGet()
        {
			UserDis = await userManager.GetUserAsync(User);

			return Page();
		}
    }
}
