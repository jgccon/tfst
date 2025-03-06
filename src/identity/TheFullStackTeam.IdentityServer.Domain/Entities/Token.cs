using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheFullStackTeam.IdentityServer.Domain.Entities
{
    public class Token
    {
        [Key]
        public string Value { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime ExpireDate { get; set; }

        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
    }
}
