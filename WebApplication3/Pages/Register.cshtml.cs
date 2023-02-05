using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.ViewModels;
using WebApplication3.Model;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Text.Encodings.Web;
using WebApplication3.Services;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private AuthDbContext authDbContext { get; }
        private readonly HtmlEncoder htmlEncoder;
		private readonly UserPasswordHistoryService _userpasswordhistory;
        private readonly RoleManager<IdentityRole> _roleManager;




        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AuthDbContext authDbContext,
        HtmlEncoder htmlEncoder,
		UserPasswordHistoryService userpasswordhistory,
        RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authDbContext = authDbContext;
            this.htmlEncoder = htmlEncoder;
            this._userpasswordhistory = userpasswordhistory;
            _roleManager = roleManager;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var existuser = await userManager.FindByEmailAsync(RModel.Email);
            if (existuser != null)
            {
                ModelState.AddModelError("", "Email is taken");
                return Page();
            }

            string[] roleNames = { "Administrator", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (ModelState.IsValid)
            {
                //ApplicationUser user = authDbContext.AspNetUsers.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                //ApplicationUser emil = db.UserProfiles.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower());
                
                byte[] imageData;
                using (var stream = new MemoryStream())
                {
                    await RModel.ImageFile.CopyToAsync(stream);
                    imageData = stream.ToArray();
                }
                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    Credit_Card = Encrypt(RModel.CreditCard),
                    Gender = RModel.Gender,
                    PhoneNumber = RModel.PhoneNumber,
                    ImageFile = imageData,
                    About = htmlEncoder.Encode(RModel.AboutMe),
                    Address = RModel.Address,
                    Full_Name = RModel.Full_Name

                };
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    
                    _userpasswordhistory.SavePassword(RModel.Email,RModel.Password,DateTime.Now);
                    await userManager.AddToRoleAsync(user, "User");
                    await signInManager.SignInAsync(user, false);
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
            }
            return Page();
        }

        private string Encrypt(string clearText)
        {
            string encryptionKey = "E)H@McQfThWmZq4t7w!z%C*F-JaNdRgU";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }

            return clearText;
        }

        








    }
}
