using System.Diagnostics.CodeAnalysis;
using SharedKernel.Errors;

namespace SharedKernel;

public class Result
{
    protected Result(bool isSuccess, DomainError domainError)
    {
        if (isSuccess && domainError != DomainError.None ||
            !isSuccess && domainError == DomainError.None)
        {
            throw new ArgumentException("Invalid error", nameof(domainError));
        }

        IsSuccess = isSuccess;
        DomainError = domainError;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError DomainError { get; }

    public static Result Success() => new(true, DomainError.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, DomainError.None);

    public static Result Failure(DomainError domainError) => new(false, domainError);

    public static Result<TValue> Failure<TValue>(DomainError domainError) =>
        new(default, false, domainError);
}

public class Result<TValue>(TValue? value, bool isSuccess, DomainError domainError) : Result(isSuccess, domainError)
{
    public TValue Value => IsSuccess
        ? value!
        : throw new InvalidOperationException("The value of a failure result can't be accessed.");

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(new NullValueError());
}
