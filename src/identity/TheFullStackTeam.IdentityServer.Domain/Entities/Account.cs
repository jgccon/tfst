using Microsoft.AspNetCore.Identity;

namespace TheFullStackTeam.IdentityServer.Domain.Entities
{
    public class Account : IdentityUser
    {
        public bool EmailVerified { get; private set; }
        public DateTime? LastLoginDate { get; set; }
        public void MarkAsVerified()
        {
            EmailVerified = true;
        }
    }
}
