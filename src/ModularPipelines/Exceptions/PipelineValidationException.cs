using ModularPipelines.Validation;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Thrown when pipeline validation fails with one or more validation errors.
/// </summary>
/// <remarks>
/// <para>
/// This exception is thrown when the pipeline validation system detects configuration
/// or setup errors before the pipeline can begin execution. Validation ensures that
/// modules are properly configured and dependencies are correctly defined.
/// </para>
/// <para><b>When this is thrown:</b></para>
/// <list type="bullet">
/// <item>During pipeline startup when validation rules are checked</item>
/// <item>When module configurations fail validation constraints</item>
/// <item>When custom validators report errors</item>
/// </list>
/// <para><b>Properties available:</b></para>
/// <list type="bullet">
/// <item><see cref="ValidationResult"/> - Contains all validation errors that occurred</item>
/// </list>
/// <para><b>Handling example:</b></para>
/// <code>
/// try
/// {
///     await pipelineHost.ExecuteAsync();
/// }
/// catch (PipelineValidationException ex)
/// {
///     Console.WriteLine($"Validation failed with {ex.ValidationResult.Errors.Count} error(s):");
///     foreach (var error in ex.ValidationResult.Errors)
///     {
///         Console.WriteLine($"  - {error}");
///     }
/// }
/// </code>
/// <para><b>Resolution:</b></para>
/// <para>
/// Review the <see cref="ValidationResult"/> to identify specific validation errors
/// and fix the pipeline configuration accordingly.
/// </para>
/// </remarks>
/// <seealso cref="PipelineException"/>
/// <seealso cref="FailedRequirementsException"/>
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
