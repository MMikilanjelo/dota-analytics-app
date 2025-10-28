namespace SharedKernel.Errors;

public sealed record NullValueError() : DomainError("General.NullValue", "Null value passed");