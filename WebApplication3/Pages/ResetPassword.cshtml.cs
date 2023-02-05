using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Web;
using WebApplication3.Model;
using WebApplication3.Services;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
	[ValidateAntiForgeryToken]
	public class ResetPasswordModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly UserPasswordHistoryService _userpasswordhistory;
		private readonly IDataProtector _dataProtector;
		[BindProperty]
		public ResetPassword RPModel { get; set; }

		[BindProperty]
		public string UserId { get; set; }

		[BindProperty]
		public string Token { get; set; }
		public ResetPasswordModel(UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger, UserPasswordHistoryService userpasswordhistory, IDataProtectionProvider dataProtectionProvider)
		{
			_userManager = userManager;
			_userpasswordhistory = userpasswordhistory;
			_dataProtector = dataProtectionProvider.CreateProtector("Your unique key");
		}
		public IActionResult OnGet(string userId, string token)
		{
			UserId = HttpUtility.UrlDecode(userId);
			//RPModel.Email = UserId;
			Token = HttpUtility.UrlDecode(token);
			Console.WriteLine("UserId in get" + UserId);
			Console.WriteLine("Token in get" + Token);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (ModelState.IsValid)
			{
				if (UserId != null)
				{
					Console.WriteLine("UserId in post" + UserId);
					Console.WriteLine("Token in post" + Token);
					var user = await _userManager.FindByEmailAsync(UserId);
					Console.WriteLine("User Email RIGHT HEREEE" + user.Email);
					if (user.Email != null)
					{
						var password_check = _userpasswordhistory.CheckPassword(user.Email, RPModel.Password);
						if (!password_check)
						{
							var result = await _userManager.ResetPasswordAsync(user, Token, RPModel.Password);
							if (result.Succeeded)
							{
								var counter = _userpasswordhistory.checkPasswordCount(UserId);
								if (counter < 2)
								{
									_userpasswordhistory.SavePassword(user.Email, RPModel.Password, DateTime.Now);
									return RedirectToPage("Login");
								}
								if (counter == 2)
								{
									_userpasswordhistory.DeletePassword(user.Email);
									_userpasswordhistory.SavePassword(user.Email, RPModel.Password, DateTime.Now);
									return RedirectToPage("Login");
								}
							}
							else
							{
								ModelState.AddModelError("", "Your password cannot be changed. Please resend another link and try again.");
							}
						}
						ModelState.AddModelError("", "This password has been used before. Please use another password.");

					}
					else
					{
						ModelState.AddModelError("", "Email does not exist.");
					}
				}

			}
			return Page();
		}
	}
}

