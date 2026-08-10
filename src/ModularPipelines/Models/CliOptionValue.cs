using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Models;

/// <summary>
/// Represents either a bare CLI option or an option with an explicit value.
/// </summary>
public sealed record CliOptionValue
{
    private CliOptionValue(string? value, bool isBare)
    {
        Value = value;
        IsBare = isBare;
    }

    /// <summary>
    /// Gets a CLI option with no value.
    /// </summary>
    public static CliOptionValue Bare { get; } = new(value: null, isBare: true);

    /// <summary>
    /// Gets the explicit option value, or <see langword="null"/> when <see cref="IsBare"/> is true.
    /// </summary>
    public string? Value { get; }

    /// <summary>
    /// Gets a value indicating whether only the option name should be rendered.
    /// </summary>
    public bool IsBare { get; }

    /// <summary>
    /// Creates an explicit option value from a string, or preserves a null value so the option is omitted.
    /// </summary>
    /// <param name="value">The value to render.</param>
    [return: NotNullIfNotNull(nameof(value))]
    public static implicit operator CliOptionValue?(string? value)
    {
        if (value is null)
        {
            return null;
        }

        return new CliOptionValue(value, isBare: false);
    }
}
