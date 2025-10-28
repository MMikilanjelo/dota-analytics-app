using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Modules.ExternalAccounts.Application.UseCases.Commands;
using Modules.ExternalAccounts.Application.UseCases.Queries;
using Modules.ExternalAccounts.Infrastructure.Extensions;
using SharedKernel.Contracts.Messaging;

namespace Modules.ExternalAccounts.Api;

public static class SteamEndpoints
{
    public static IEndpointRouteBuilder MapSteamEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/external/steam/login",
                async (
                    [FromQuery(Name = "redirect_uri")] string redirectUri,
                    HttpContext ctx,
                    IQueryHandler<GetSteamLoginUrlQuery, string> handler,
                    CancellationToken ct
                ) =>
                {
                    var query = new GetSteamLoginUrlQuery(
                        ctx.Request.Scheme,
                        ctx.Request.Host.Value,
                        ctx.Request.PathBase.Value,
                        redirectUri
                    );

                    var result = await handler.HandleAsync(query, ct);

                    return result.IsSuccess
                        ? Results.Ok(new { url = result.Value })
                        : CustomResults.Problem(result);
                })
            .RequireAuthorization();

        app.MapGet("/api/external/steam/callback",
            async (
                HttpContext ctx,
                ICommandHandler<LinkSteamAccountCommand> handler,
                CancellationToken ct
            ) =>
            {
                var queryParams = ctx.Request.Query.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

                var command = new LinkSteamAccountCommand(queryParams);

                var result = await handler.HandleAsync(command, ct);

                return result.IsSuccess
                    ? Results.Ok()
                    : CustomResults.Problem(result);
            });

        return app;
    }
}