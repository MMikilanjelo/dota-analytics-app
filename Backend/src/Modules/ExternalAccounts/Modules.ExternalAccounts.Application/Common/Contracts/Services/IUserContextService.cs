using StrictId;

namespace Modules.ExternalAccounts.Application.Common.Contracts.Services;

public interface IUserContextService
{
    Id UserId { get; }
}