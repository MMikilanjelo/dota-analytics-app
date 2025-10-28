namespace SharedKernel.Errors;

public abstract record DomainError(string Code, string Message)
{
    public static readonly DomainError None = new NoneError();
}