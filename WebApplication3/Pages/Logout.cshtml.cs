using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using WebApplication3.Model;
using WebApplication3.Services;

namespace WebApplication3.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private UserManager<ApplicationUser> userManager { get; }
        private readonly IAuditLogService _auditLogService;


        public LogoutModel(SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager, 
            IAuditLogService auditLogService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _auditLogService= auditLogService;
        }

        public async Task<IActionResult> OnGet()
        {
            string userid = userManager.GetUserId(User);
            await signInManager.SignOutAsync();

            var log = new AuditLog
            {
                UserId = userid,
                Action = "LOGOUT",
                Timestamp = DateTime.UtcNow
            };
            await _auditLogService.LogAsync(log);

            return RedirectToPage("Login");
        }
    }
}
