namespace ModularPipelines.Validation;

/// <summary>
/// Represents the result of a validation operation.
/// </summary>
public class ValidationResult
{
    private readonly List<ValidationError> _errors = [];

    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public IReadOnlyList<ValidationError> Errors => _errors;

    /// <summary>
    /// Gets a value indicating whether the validation has any errors.
    /// </summary>
    public bool HasErrors => _errors.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the validation passed without errors.
    /// </summary>
    public bool IsValid => _errors.Count == 0;

    /// <summary>
    /// Adds an error to the validation result.
    /// </summary>
    /// <param name="error">The validation error to add.</param>
    public void AddError(ValidationError error)
    {
        _errors.Add(error);
    }

    /// <summary>
    /// Adds multiple errors to the validation result.
    /// </summary>
    /// <param name="errors">The validation errors to add.</param>
    public void AddErrors(IEnumerable<ValidationError> errors)
    {
        _errors.AddRange(errors);
    }

    /// <summary>
    /// Creates a successful validation result with no errors.
    /// </summary>
    /// <returns>A new ValidationResult with no errors.</returns>
    public static ValidationResult Success() => new();

    /// <summary>
    /// Creates a validation result with a single error.
    /// </summary>
    /// <param name="error">The validation error.</param>
    /// <returns>A new ValidationResult containing the specified error.</returns>
    public static ValidationResult WithError(ValidationError error)
    {
        var result = new ValidationResult();
        result.AddError(error);
        return result;
    }

    /// <summary>
    /// Creates a validation result with multiple errors.
    /// </summary>
    /// <param name="errors">The validation errors.</param>
    /// <returns>A new ValidationResult containing the specified errors.</returns>
    public static ValidationResult WithErrors(IEnumerable<ValidationError> errors)
    {
        var result = new ValidationResult();
        result.AddErrors(errors);
        return result;
    }

    /// <summary>
    /// Merges another validation result into this one.
    /// </summary>
    /// <param name="other">The validation result to merge.</param>
    public void Merge(ValidationResult other)
    {
        _errors.AddRange(other.Errors);
    }
}
