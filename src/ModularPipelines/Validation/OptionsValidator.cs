using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Validation;

/// <summary>
/// Validates pipeline options configuration.
/// </summary>
internal class OptionsValidator : IOptionsValidator
{
    /// <inheritdoc />
    public int Order => 100;

    /// <inheritdoc />
    public ValidationResult Validate(IServiceProvider services)
    {
        var optionsSnapshot = services.GetService<IOptions<PipelineOptions>>();
        if (optionsSnapshot?.Value == null)
        {
            return ValidationResult.Success();
        }

        return ValidateOptions(optionsSnapshot.Value);
    }

    /// <inheritdoc />
    public ValidationResult ValidateOptions(PipelineOptions options)
    {
        var result = new ValidationResult();

        // Validate DefaultRetryCount
        if (options.DefaultRetryCount < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"DefaultRetryCount cannot be negative. Current value: {options.DefaultRetryCount}"));
        }

        // Validate concurrency options
        if (options.Concurrency.MaxParallelism < 1)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.MaxParallelism must be at least 1. Current value: {options.Concurrency.MaxParallelism}"));
        }

        // Validate HTTP timeout if set
        if (options.DefaultHttpTimeout.HasValue && options.DefaultHttpTimeout.Value <= TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"DefaultHttpTimeout must be positive. Current value: {options.DefaultHttpTimeout.Value}"));
        }

        // Validate conflicting category filters
        if (options.RunOnlyCategories != null && options.IgnoreCategories != null)
        {
            var conflicts = options.RunOnlyCategories.Intersect(options.IgnoreCategories).ToList();
            if (conflicts.Count > 0)
            {
                result.AddError(new ValidationError(
                    ValidationErrorCategory.Options,
                    $"Categories cannot be in both RunOnlyCategories and IgnoreCategories: {string.Join(", ", conflicts)}"));
            }
        }

        return result;
    }
}
