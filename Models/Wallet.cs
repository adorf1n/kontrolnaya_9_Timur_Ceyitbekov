using Microsoft.AspNetCore.Identity;

namespace WebApplication.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public double Balance { get; set; }
        public IdentityUser User { get; set; }
    }
}
