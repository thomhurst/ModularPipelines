using ModularPipelines.Options;

namespace ModularPipelines.Validation;

/// <summary>
/// Interface for validating pipeline options configuration.
/// </summary>
internal interface IOptionsValidator : IPipelineValidator
{
    /// <summary>
    /// Validates pipeline option values that do not depend on registered modules.
    /// </summary>
    /// <param name="options">The pipeline options to validate.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult ValidateOptions(PipelineOptions options);

    /// <summary>
    /// Validates the pipeline options, including category filters against registered categories.
    /// </summary>
    /// <param name="options">The pipeline options to validate.</param>
    /// <param name="registeredCategories">The categories assigned to registered modules.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult ValidateOptions(
        PipelineOptions options,
        IReadOnlySet<string> registeredCategories) => ValidateOptions(options);
}
