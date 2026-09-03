namespace ModularPipelines.Go.Options;

/// <summary>
/// Represents one ordered mutation for <c>go mod edit</c> or <c>go work edit</c>.
/// </summary>
public sealed record GoEditOperation
{
    /// <summary>
    /// Initializes an ordered Go edit operation.
    /// </summary>
    /// <param name="option">The edit option, including its leading hyphen.</param>
    /// <param name="value">The option value.</param>
    public GoEditOperation(string option, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(option);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        if (option.Length < 2
            || option[0] != '-'
            || option.Contains('=')
            || option.Any(char.IsWhiteSpace))
        {
            throw new ArgumentException(
                "A Go edit option must be a single hyphen-prefixed name without '='.",
                nameof(option));
        }

        Option = option;
        Value = value;
    }

    /// <summary>
    /// Gets the edit option, including its leading hyphen.
    /// </summary>
    public string Option { get; }

    /// <summary>
    /// Gets the option value.
    /// </summary>
    public string Value { get; }

    /// <inheritdoc />
    public override string ToString() => $"{Option}={Value}";
}
