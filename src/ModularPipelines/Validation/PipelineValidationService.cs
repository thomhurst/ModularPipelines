using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Validation;

/// <summary>
/// Service that orchestrates all pipeline validation.
/// </summary>
internal class PipelineValidationService : IPipelineValidationService
{
    /// <summary>
    /// Validates the pipeline configuration using all registered validators.
    /// </summary>
    /// <param name="services">The service provider containing registered services.</param>
    /// <returns>A validation result containing all errors found.</returns>
    public async Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        var result = new ValidationResult();

        // Get all registered validators and run them in order
        var validators = services.GetServices<IPipelineValidator>()
            .OrderBy(v => v.Order)
            .ToList();

        foreach (var validator in validators)
        {
            var validatorResult = await validator.ValidateAsync(services).ConfigureAwait(false);
            result.Merge(validatorResult);
        }

        return result;
    }
}
