using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication3.ViewModels;
using WebApplication3.Model;

namespace WebApplication3.Pages
{
    public class RegisterModel : PageModel
    {

        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }
        private AuthDbContext authDbContext { get; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        AuthDbContext authDbContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authDbContext = authDbContext;
        }



        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
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
                    Credit_Card = RModel.CreditCard,
                    Gender = RModel.Gender,
                    PhoneNumber = RModel.PhoneNumber,
                    ImageFile = imageData,
                    About = RModel.AboutMe,
                    Address = RModel.Address

                };
                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
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







    }
}
