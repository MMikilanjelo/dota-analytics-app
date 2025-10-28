namespace SharedKernel.Errors;

public sealed record ProblemError(string Code, string Message) : DomainError(Code, Message);