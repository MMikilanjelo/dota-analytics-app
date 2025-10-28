using Common.Contracts;
using Modules.ExternalAccounts.Domain.ValueObjects;
using StrictId;

namespace Modules.ExternalAccounts.Domain.Aggregates;

public interface IExternalAccountLinkRepository : IRepository
{
    void AddExternalAccount(ExternalAccountLinkEntity entity, CancellationToken cancellationToken);
    Task<ExternalAccountLinkEntity?> GetExternalAccountLink(Id ownerId, ExternalAccountId externalAccountId, CancellationToken cancellationToken);
    void UpdateExternalAccount(ExternalAccountLinkEntity existingAccount, CancellationToken cancellationToken);
}