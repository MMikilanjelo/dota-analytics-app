using System.Text.RegularExpressions;
using Common.Abstractions;

namespace Modules.Users.Domain.ValueObjects;

public class Email : ValueObject
{
    public string Value { get; private set; }

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        if (!EmailRegex.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
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