namespace SharedKernel.Errors;

public sealed record NoneError() : DomainError("General.None", "No error");