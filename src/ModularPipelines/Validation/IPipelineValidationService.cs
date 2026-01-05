namespace ModularPipelines.Validation;

/// <summary>
/// Service interface for orchestrating pipeline validation.
/// </summary>
public interface IPipelineValidationService
{
    /// <summary>
    /// Validates the pipeline configuration using all registered validators.
    /// </summary>
    /// <param name="services">The service provider containing registered services.</param>
    /// <returns>A validation result containing all errors found.</returns>
    ValidationResult Validate(IServiceProvider services);
}
