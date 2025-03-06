using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheFullStackTeam.IdentityServer.Domain.Entities
{
    public class ExternalProviderCredential
    {
        [Key]
        public string Provider { get; set; } = string.Empty;
        public string ProviderId { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;

        [ForeignKey(nameof(AccountId))]
        public Account? Account { get; set; }
    }
}
