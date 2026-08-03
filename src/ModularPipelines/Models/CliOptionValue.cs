namespace ModularPipelines.Models;

/// <summary>
/// Represents either a bare CLI option or an option with an explicit value.
/// </summary>
public readonly record struct CliOptionValue
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
    /// Creates an option value from a non-empty string.
    /// </summary>
    /// <param name="value">The value to render.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="value"/> is empty or whitespace.</exception>
    public static implicit operator CliOptionValue(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        return new CliOptionValue(value, isBare: false);
    }
}
