namespace ModularPipelines.Validation;

/// <summary>
/// Base interface for pipeline validators.
/// </summary>
/// <remarks>
/// Validators run while the pipeline is built, in ascending <see cref="Order"/>.
/// Register custom validators with <c>AddValidator&lt;TValidator&gt;()</c>.
/// </remarks>
public interface IPipelineValidator
{
    /// <summary>
    /// Gets the order in which this validator runs relative to other validators.
    /// Lower values run first. The default is 0; built-in validators use 100 to 300.
    /// </summary>
    int Order => 0;

    /// <summary>
    /// Asynchronously validates the pipeline configuration.
    /// </summary>
    /// <param name="services">The service provider containing registered services.</param>
    /// <param name="cancellationToken">A token that cancels validation.</param>
    /// <returns>A validation result containing any errors found.</returns>
    Task<ValidationResult> ValidateAsync(IServiceProvider services, CancellationToken cancellationToken);
}
