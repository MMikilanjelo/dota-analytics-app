using System.Text.RegularExpressions;
using SharedKernel;
using SharedKernel.Abstractions;
using SharedKernel.Errors;

namespace Modules.Users.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; private set; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<Email>(new ValidationError([new NullValueError()]));
        }

        if (!EmailRegex.IsMatch(value))
        {
            return Result.Failure<Email>(new ValidationError([new InvalidValueFormatError()]));
        }

        return new Email(value.Trim());
    }

    public override string ToString() => Value;

    private Email(string value)
    {
        Value = value;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}