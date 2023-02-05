using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Model
{
	public class UserPasswordHistory
	{
		[Key]
		public int id { get; set; }
		public string UserEmail { get; set; }
		public string Password { get; set; }
		public DateTime Password_Date { get; set; }
	}
}
