using Common.Contracts;
using Modules.ExternalAccounts.Domain.ValueObjects;
using StrictId;

namespace Modules.ExternalAccounts.Domain.Aggregates;

public class ExternalAccountLinkEntity : AggreggateRoot
{
    public required Id<ExternalAccountLinkEntity> Id { get; init; } = Id<ExternalAccountLinkEntity>.Empty;
    public required Id OwnerId { get; init; } = new();
    public required ExternalAccountId ExternalAccountId { get; init; }
    public required DateTime LinkedAtUtc { get; init; }
    public DateTime LastSyncedAtUtc { get; private set; }
    public TimeSpan SyncInterval { get; private set; }

    public bool HasBeenSynced =>
        LastSyncedAtUtc > DateTime.UtcNow;

    public void RefreshLink(TimeSpan newInterval)
    {
        SyncInterval = newInterval;

        LastSyncedAtUtc = DateTime.UtcNow;
    }

    public static ExternalAccountLinkEntity CreateSteamLink(
        Id ownerId,
        TimeSpan maxAge,
        SteamId externalAccountId
    )
    {
        return new ExternalAccountLinkEntity
        {
            Id = Id<ExternalAccountLinkEntity>.NewId(),
            OwnerId = ownerId,
            LinkedAtUtc = DateTime.UtcNow,
            SyncInterval = maxAge,
            ExternalAccountId = externalAccountId,
            LastSyncedAtUtc = DateTime.UtcNow
        };
    }

    private ExternalAccountLinkEntity()
    {
    }
}