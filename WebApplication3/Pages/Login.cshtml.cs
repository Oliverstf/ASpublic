using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.Model;
using WebApplication3.ViewModels;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Azure;
using System.Text.Json.Serialization;
using System.Net;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.Services;

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuditLogService _auditLogService;

		private readonly IOptions<RecaptchaSettings> _recaptchaSettings;
		private readonly IHttpClientFactory _httpClientFactory;
		public LoginModel(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
						  IOptions<RecaptchaSettings> recaptchaSettings,
			              IHttpClientFactory httpClientFactory, 
                          IAuditLogService auditLogService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _auditLogService= auditLogService;
        }

		public class RecaptchaSettings
		{
			public string SecretKey { get; } = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe";
		}

		public class RecaptchaResponse
		{
			public bool Success { get; set; }
			public string[] ErrorCodes { get; set; }
		}

		public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
			if (ModelState.IsValid && ReCaptchaPassed(Request.Form["foo"]))
            {
                //PasswordSignInAsync uses Username and password not email and password
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Username, LModel.Password,
                false, false);
                if (identityResult.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(LModel.Username);
                    var userId = user.Id;
                    var log = new AuditLog
                    {
                        UserId = userId,
                        Action = "LOGIN",
                        Timestamp = DateTime.UtcNow
                    };
                    await _auditLogService.LogAsync(log);
                    return Redirect("/");

                }
                else
                {
                    ModelState.AddModelError("", "Incorerct Username or password");
                }
            }
            if (!ReCaptchaPassed(Request.Form["foo"]))
            {
                ModelState.AddModelError(string.Empty, "You failed the CAPTCHA.");
                return Page();
            }
            return Page();
        }

		public static bool ReCaptchaPassed(string gRecaptchaResponse)
		{
			HttpClient httpClient = new HttpClient();

			var res = httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe&response={gRecaptchaResponse}").Result;

			if (res.StatusCode != HttpStatusCode.OK)
			{
				return false;
			}
			string JSONres = res.Content.ReadAsStringAsync().Result;
			dynamic JSONdata = JObject.Parse(JSONres);

			if (JSONdata.success != "true" || JSONdata.score <= 0.5m)
			{
				return false;
			}

			return true;
		}
	}
}
