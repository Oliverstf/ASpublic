using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApplication3.ViewModels
{
    public class Register
    {

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{12,}$", ErrorMessage = "Please use at least 12 characters, at least one lowercase, uppercase, digit and special character.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [RegularExpression("^4[0-9]{12}(?:[0-9]{3})?$", ErrorMessage = "Invalid Card")]
        public string CreditCard { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public IFormFile ImageFile { get; set; }
        [FileExtensions(Extensions = "jpg,jpeg", ErrorMessage ="Invalid File Extention")]
        public string FileName => ImageFile?.FileName;
        [Required]
        public string AboutMe { get; set; }
        [Required]
        public string Gender { get; set; }





    }
}
