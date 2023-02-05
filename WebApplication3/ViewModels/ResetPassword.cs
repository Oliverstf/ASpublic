using System.ComponentModel.DataAnnotations;

namespace WebApplication3.ViewModels
{
	public class ResetPassword
	{
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[#$^+=!*()@%&]).{12,}$", ErrorMessage = "Please use at least 12 characters, at least one lowercase, uppercase, digit and special character.")]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
		public string ConfirmPassword { get; set; }
	}
}
