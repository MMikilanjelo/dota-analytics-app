using Common.Contracts;
using Microsoft.EntityFrameworkCore;
using Modules.ExternalAccounts.Domain;
using Modules.ExternalAccounts.Domain.Aggregates;
using Modules.ExternalAccounts.Domain.ValueObjects;
using StrictId;

namespace Modules.ExternalAccounts.Infrastructure.Data.Repositories;

public class ExternalAccountLinkRepository(ExternalAccountsDbContext dbContext, IExternalAccountsUnitOfWork externalAccountsUnitOfWork) : IExternalAccountLinkRepository
{
    public IUnitOfWork UnitOfWork => externalAccountsUnitOfWork;

    public void AddExternalAccount(ExternalAccountLinkEntity entity, CancellationToken cancellationToken)
    {
        dbContext.Add(entity);
    }

    public void UpdateExternalAccount(ExternalAccountLinkEntity existingAccount, CancellationToken cancellationToken)
    {
        dbContext.Accounts.Update(existingAccount);
    }

    public Task<ExternalAccountLinkEntity?> GetExternalAccountLink(Id ownerId, ExternalAccountId externalAccountId, CancellationToken cancellationToken)
    {
        var entity = dbContext.Accounts.FirstOrDefaultAsync(
            a => a.OwnerId == ownerId &&
                 a.ExternalAccountId == externalAccountId,
            cancellationToken
        );

        return entity;
    }
}