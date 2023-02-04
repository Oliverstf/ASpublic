using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace WebApplication3.Model
{
    public class ErrorDesc : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName) { 
            return new IdentityError { Code = nameof(DuplicateUserName), 
                Description = $"Email '{userName}' is already taken." }; }
    }
}
