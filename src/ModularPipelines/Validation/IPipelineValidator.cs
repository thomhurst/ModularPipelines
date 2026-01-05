namespace ModularPipelines.Validation;

/// <summary>
/// Base interface for pipeline validators.
/// </summary>
public interface IPipelineValidator
{
    /// <summary>
    /// Gets the order in which this validator should run. Lower values run first.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Validates the pipeline configuration.
    /// </summary>
    /// <param name="services">The service provider containing registered services.</param>
    /// <returns>A validation result containing any errors found.</returns>
    ValidationResult Validate(IServiceProvider services);
}
