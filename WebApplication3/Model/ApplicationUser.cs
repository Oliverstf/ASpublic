using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Credit_Card { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public byte[] ImageFile { get; set; }
        [NotMapped]
        public string UnenCredit_Card { get; set; }



    }
}
