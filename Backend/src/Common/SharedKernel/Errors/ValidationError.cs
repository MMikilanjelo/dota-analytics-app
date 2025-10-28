namespace SharedKernel.Errors;

public sealed record ValidationError(DomainError[] Errors) : DomainError("Validation.General", "One or more validation errors occurred");