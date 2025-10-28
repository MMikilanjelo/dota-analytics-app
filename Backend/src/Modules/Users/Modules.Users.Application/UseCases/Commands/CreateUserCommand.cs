using Common;
using Common.Contracts.Events;
using Common.Contracts.Messaging;
using FluentValidation;
using Modules.Users.Application.UseCases.Contracts.DataLoaders;
using Modules.Users.Application.UseCases.Contracts.Services;
using Modules.Users.Domain.Aggregates.UserAggregate;
using Modules.Users.Domain.Aggregates.UserAggregate.Events;
using Modules.Users.IntegrationEvents;
using User = Modules.Users.Application.Common.Models.User;

namespace Modules.Users.Application.UseCases.Commands;

public sealed record CreateUserCommand(string Email, string Password) : ICommand<User>;

public sealed record NotUniqueEmailError : DomainError
{
    public NotUniqueEmailError(string email) : base("Users.NotUniqueEmail", $"Email '{email}' is already in use.")
    {
    }
}

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUserByEmailDataLoader userByEmailDataLoader,
    IPasswordHasherService passwordHasherService,
    IEventPublisher eventBus
) : ICommandHandler<CreateUserCommand, User>
{
    public async Task<Result<User>> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await userByEmailDataLoader.LoadAsync(command.Email, cancellationToken);

        if (existingUser != null)
        {
            return Result.Failure<User>(new NotUniqueEmailError(command.Email));
        }

        var passwordHash = passwordHasherService.Hash(command.Password);

        var user = UserEntity.Create(command.Email, passwordHash);

        userRepository.AddUser(user, cancellationToken);

        await eventBus.Publish(new UserCreatedDomainEvent(user.Id), cancellationToken);

        await userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new User
        {
            Id = user.Id.ToString(),
            Email = user.Email.ToString()
        });
    }
}

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}