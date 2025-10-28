namespace Common;

public abstract record DomainError(string Code, string Message)
{
    public static readonly DomainError None = new NoneError();
}

public sealed record NoneError() : DomainError("General.None", "No error");

public sealed record NullValueError() : DomainError("General.NullValue", "Null value passed");

public sealed record ProblemError(string Code, string Message) : DomainError(Code, Message);

public sealed record UnknownError() : DomainError("General.Unknown", "Something went wrong");

public sealed record ValidationError(DomainError[] Errors) : DomainError("Validation.General", "One or more validation errors occurred");