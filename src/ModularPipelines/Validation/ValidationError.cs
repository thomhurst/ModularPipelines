namespace ModularPipelines.Validation;

/// <summary>
/// Represents a validation error with details about what went wrong.
/// </summary>
public record ValidationError
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ValidationError"/> class.
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="category">The category of the validation error.</param>
    /// <param name="message">The error message describing the validation failure.</param>
    /// <param name="sourceType">The optional source type that caused the validation error.</param>
    public ValidationError(ValidationErrorCategory category, string message, Type? sourceType = null)
    {
        Category = category;
        Message = message;
        SourceType = sourceType;
    }

    /// <summary>
    /// Gets the category of the validation error.
    /// </summary>
    public ValidationErrorCategory Category { get; }

    /// <summary>
    /// Gets the error message describing the validation failure.
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the source type that caused the validation error, if applicable.
    /// </summary>
    public Type? SourceType { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        var source = SourceType != null ? $" [{SourceType.Name}]" : string.Empty;
        return $"[{Category}]{source}: {Message}";
    }
}
