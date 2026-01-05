using ModularPipelines.Options;

namespace ModularPipelines.Validation;

/// <summary>
/// Interface for validating pipeline options configuration.
/// </summary>
public interface IOptionsValidator : IPipelineValidator
{
    /// <summary>
    /// Validates the pipeline options.
    /// </summary>
    /// <param name="options">The pipeline options to validate.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult ValidateOptions(PipelineOptions options);
}
