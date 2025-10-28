using Modules.Users.Domain.Aggregates.UserAggregate;

namespace Modules.Users.Application.UseCases.Contracts.Services;

public interface IIdentityService
{
    string GenerateAccessToken(UserEntity userEntity);
    string GenerateRefreshToken();
}