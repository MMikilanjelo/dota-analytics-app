namespace SharedKernel.Errors;

public sealed record UnknownError() : DomainError("General.Unknown", "Something went wrong");