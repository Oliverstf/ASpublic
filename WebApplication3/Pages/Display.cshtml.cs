using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    [Authorize]
    public class DisplayModel : PageModel
    {
		private UserManager<ApplicationUser> userManager { get; }
        private readonly HtmlEncoder htmlEncoder;
        public DisplayModel(UserManager<ApplicationUser> userManager, HtmlEncoder htmlEncoder)
		{
			this.userManager = userManager;
            this.htmlEncoder = htmlEncoder;
        }
		[BindProperty]
		public ApplicationUser UserDis { get; set; }
        public string imgSrc { get; set; }
        public string About { get; set; }

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (!context.HttpContext.User.IsInRole("Administrator"))
            {
                context.Result = new RedirectToPageResult("/Login");
            }

            base.OnPageHandlerExecuting(context);
        }
        public async Task<IActionResult> OnGet()
        {
			UserDis = await userManager.GetUserAsync(User);
            var base64 = Convert.ToBase64String(UserDis.ImageFile);
            imgSrc = string.Format("data:image/jpg;base64,{0}", base64);
            UserDis.UnenCredit_Card = Decrypt(UserDis.Credit_Card);
            About = UserDis.About.ToString();


            return Page();
		}

        private string Decrypt(string cipherText)
        {
            string encryptionKey = "E)H@McQfThWmZq4t7w!z%C*F-JaNdRgU";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return cipherText;
        }
    }

    
}
