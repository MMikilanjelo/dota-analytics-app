using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Modules.ExternalAccounts.Application.Common.Contracts.Services;
using StrictId;

namespace Modules.ExternalAccounts.Infrastructure.Services;

public class UserContextService(IHttpContextAccessor httpContextAccessor) : IUserContextService
{
    public Id UserId
    {
        get
        {
            var id = GetUserId(httpContextAccessor.HttpContext?.User);

            if (id.HasValue)
            {
                return id;
            }

            throw new ApplicationException("User context is unavailable");
        }
    }

    private static Id GetUserId(ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Id.TryParse(userId, out var id) ? id : Id.Empty;
    }
}