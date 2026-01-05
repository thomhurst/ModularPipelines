using ModularPipelines.Validation;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when pipeline validation fails.
/// </summary>
public class PipelineValidationException : PipelineException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PipelineValidationException"/> class.
    /// </summary>
    /// <param name="validationResult">The validation result containing the errors.</param>
    public PipelineValidationException(ValidationResult validationResult)
        : base(FormatMessage(validationResult))
    {
        ValidationResult = validationResult;
    }

    /// <summary>
    /// Gets the validation result containing all errors.
    /// </summary>
    public ValidationResult ValidationResult { get; }

    private static string FormatMessage(ValidationResult result)
    {
        if (!result.HasErrors)
        {
            return "Pipeline validation failed with no specific errors.";
        }

        var errorMessages = result.Errors.Select(e => $"  - {e}");
        return $"Pipeline validation failed with {result.Errors.Count} error(s):{Environment.NewLine}{string.Join(Environment.NewLine, errorMessages)}";
    }
}
