using SharedKernel;
using SharedKernel.Contracts.Messaging;
using FluentValidation;
using Modules.ExternalAccounts.Application.Common.Contracts.Clients;
using Modules.ExternalAccounts.Application.Common.Contracts.Services;
using Modules.ExternalAccounts.Domain.Aggregates;
using Modules.ExternalAccounts.Domain.ValueObjects;
using StrictId;

namespace Modules.ExternalAccounts.Application.UseCases.Commands;

public sealed record LinkSteamAccountCommand(IDictionary<string, string> QueryParams) : ICommand;

internal sealed class LinkSteamAccountCommandHandler(
    ISteamClient steamClient,
    IExternalAccountLinkRepository externalAccountLinkRepository
) : ICommandHandler<LinkSteamAccountCommand>
{
    public async Task<Result> HandleAsync(LinkSteamAccountCommand command, CancellationToken cancellationToken)
    {
        var validationResult = await steamClient.ValidateCallbackAsync(command.QueryParams, cancellationToken);

        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        var createSteamIdResult = SteamId.Create(validationResult.Value.SteamId);

        if (createSteamIdResult.IsFailure)
        {
            return Result.Failure(createSteamIdResult.DomainError);
        }

        var userId = new Id(validationResult.Value.UserId);

        var existingAccount = await externalAccountLinkRepository.GetExternalAccountLink(
            userId,
            createSteamIdResult.Value,
            cancellationToken
        );

        if (existingAccount == null)
        {
            var steamLink = ExternalAccountLinkEntity.CreateSteamLink(
                userId,
                TimeSpan.FromDays(30),
                createSteamIdResult.Value
            );

            externalAccountLinkRepository.AddExternalAccount(steamLink, cancellationToken);

            await externalAccountLinkRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        existingAccount.RefreshLink(TimeSpan.FromDays(30));

        externalAccountLinkRepository.UpdateExternalAccount(existingAccount, cancellationToken);

        await externalAccountLinkRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}