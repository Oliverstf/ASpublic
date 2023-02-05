using WebApplication3.Model;

namespace WebApplication3.Services
{
	public class UserPasswordHistoryService
	{
		private readonly AuthDbContext _context;

		public UserPasswordHistoryService(AuthDbContext context)
		{
			_context = context;
		}

		public int checkPasswordCount(string userEmail)
		{
			var password_count = _context.UserPasswordHistory.Where(x => x.UserEmail == userEmail).Count();
			return password_count;
		}

		public void SavePassword(string userEmail, string password, DateTime password_date_time)
		{
			var userpassword = new UserPasswordHistory
			{
				UserEmail = userEmail,
				Password = password,
				Password_Date = password_date_time

			};
			_context.UserPasswordHistory.Add(userpassword);
			_context.SaveChanges();
		}

		public bool CheckPassword(string userEmail, string password)
		{
			var passwordHistory = _context.UserPasswordHistory.Where(p => p.UserEmail == userEmail);

			return passwordHistory.Any(p => p.Password == password);
		}

		public void DeletePassword(string userEmail)
		{
			var user = _context.UserPasswordHistory.OrderBy(u => u.Password_Date).FirstOrDefault(u => u.UserEmail == userEmail);
			if (user != null)
			{
				_context.UserPasswordHistory.Remove(user);
				_context.SaveChanges();
			}
			else
			{
				Console.WriteLine(user);
			}
		}
	}
}
