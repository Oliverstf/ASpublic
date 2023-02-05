using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;
using System.Web;
using WebApplication3.Model;
using WebApplication3.ViewModels;

namespace WebApplication3.Pages
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ForgetPasswordModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [BindProperty]
        public ForgetPassword FPModel { get; set; }

        public void OnGet()
        {
            
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (FPModel.Email != null)
                {
                    try
                    {
                        var user = await userManager.FindByEmailAsync(FPModel.Email);
                        if (user != null)
                        {
                            var email_encode = HttpUtility.UrlEncode(FPModel.Email);
                            var token = HttpUtility.UrlEncode(await userManager.GeneratePasswordResetTokenAsync(user));
                            string link = "https://localhost:44358/ResetPassword?userId={0}&token={1}";
                            link = string.Format(link, email_encode, token);
                            MailMessage mail = new MailMessage();
                            mail.To.Add(FPModel.Email.ToString().Trim());
                            mail.From = new MailAddress("olivermonteiro1410@gmail.com");
                            mail.Subject = "Hello test email :)";
                            mail.Body = "<p>Reset link here:<br/>" + link + "</p>";
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Port = 587;
                            smtp.EnableSsl = true;
                            smtp.UseDefaultCredentials = false;
                            smtp.Host = "smtp.gmail.com";
                            smtp.Credentials = new System.Net.NetworkCredential("olivermonteiro1410@gmail.com", "vxoacvnpywlabeih");
                            smtp.Send(mail);
                            return RedirectToPage("EmailSentConfirmed");
                        }
                        else
                        {
                            ModelState.AddModelError("", "You are not a registered user");
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e.ToString());
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Email is needed for the link to be sent");
                }
            }
            return Page();
        }
    }
}
