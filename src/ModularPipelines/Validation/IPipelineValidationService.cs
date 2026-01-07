namespace ModularPipelines.Validation;

/// <summary>
/// Service interface for orchestrating pipeline validation.
/// </summary>
/// <remarks>
/// This is an internal service used by the pipeline engine.
/// To implement custom validation, implement <see cref="IPipelineValidator"/> instead.
/// </remarks>
internal interface IPipelineValidationService
{
    /// <summary>
    /// Validates the pipeline configuration using all registered validators.
    /// </summary>
    /// <param name="services">The service provider containing registered services.</param>
    /// <returns>A validation result containing all errors found.</returns>
    ValidationResult Validate(IServiceProvider services);
}
