using System.Collections.Generic;

namespace LOAN_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public double Income { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
        public bool IsBlocked { get; set; } = false;
        public List<Loan> Loans { get; set; } = new List<Loan>();
    }

    public static class Role
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
