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

namespace WebApplication3.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

		private readonly IOptions<RecaptchaSettings> _recaptchaSettings;
		private readonly IHttpClientFactory _httpClientFactory;
		public LoginModel(SignInManager<ApplicationUser> signInManager,
                          UserManager<ApplicationUser> userManager,
						  IOptions<RecaptchaSettings> recaptchaSettings,
			              IHttpClientFactory httpClientFactory)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
			if (ModelState.IsValid)
            {
                //PasswordSignInAsync uses Username and password not email and password
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Username, LModel.Password,
                false, false);
                if (identityResult.Succeeded)
                {
                    return RedirectToAction("Display");

                }
                else
                {
                    ModelState.AddModelError("", "Incorerct Username or password");
                }
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
