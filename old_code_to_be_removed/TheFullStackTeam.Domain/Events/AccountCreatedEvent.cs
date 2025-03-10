using TheFullStackTeam.Domain.Entities;
using TheFullStackTeam.Domain.ValueObjects;

namespace TheFullStackTeam.Domain.Events;

public class AccountCreatedEvent : EventBase
{
    public Account Account { get; }
    public VerificationToken VerificationToken { get; }

    public AccountCreatedEvent(Account account, VerificationToken verificationToken)
    {
        Account = account ?? throw new ArgumentNullException(nameof(account));
        VerificationToken = verificationToken ?? throw new ArgumentNullException(nameof(verificationToken));
    }
}
