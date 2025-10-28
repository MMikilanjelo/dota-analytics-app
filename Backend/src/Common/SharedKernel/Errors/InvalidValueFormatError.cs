namespace SharedKernel.Errors;

public sealed record InvalidValueFormatError() : DomainError("General.InvalidValueFormat", "Invalid value passed");