using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
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

        return ValidateOptions(
            optionsSnapshot.Value,
            GetRegisteredCategories(services));
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

        if (options.DefaultModuleTimeout < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"DefaultModuleTimeout cannot be negative. Current value: {options.DefaultModuleTimeout}"));
        }

        if (options.ModuleOutputFlushInterval < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"ModuleOutputFlushInterval cannot be negative. Current value: {options.ModuleOutputFlushInterval}"));
        }
        else if (options.ModuleOutputFlushInterval > PipelineOptions.MaximumModuleOutputFlushInterval)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"ModuleOutputFlushInterval cannot exceed {PipelineOptions.MaximumModuleOutputFlushInterval}. " +
                $"Current value: {options.ModuleOutputFlushInterval}"));
        }

        if (options.ModuleOutputFlushThreshold < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"ModuleOutputFlushThreshold cannot be negative. Current value: {options.ModuleOutputFlushThreshold}"));
        }

        if (options.RunReport.HistoryRetention < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.HistoryRetention cannot be negative. Current value: " +
                $"{options.RunReport.HistoryRetention}"));
        }

        if (options.RunReport.IncludeModuleOutput
            && options.RunReport.MaxOutputBytesPerModule <= 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.MaxOutputBytesPerModule must be positive. Current value: " +
                $"{options.RunReport.MaxOutputBytesPerModule}"));
        }

        if (options.RunReport.HistoryRetention > 0
            && string.IsNullOrWhiteSpace(options.RunReport.HistoryDirectory))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "RunReport.HistoryDirectory cannot be empty when run history is enabled."));
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
            var conflicts = options.RunOnlyCategories
                .Intersect(options.IgnoreCategories, StringComparer.OrdinalIgnoreCase)
                .ToList();
            if (conflicts.Count > 0)
            {
                result.AddError(new ValidationError(
                    ValidationErrorCategory.Options,
                    $"Categories cannot be in both RunOnlyCategories and IgnoreCategories: {string.Join(", ", conflicts)}"));
            }
        }

        return result;
    }

    /// <inheritdoc />
    public ValidationResult ValidateOptions(
        PipelineOptions options,
        IReadOnlySet<string> registeredCategories)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(registeredCategories);

        var result = ValidateOptions(options);
        var normalizedRegisteredCategories =
            new HashSet<string>(registeredCategories, StringComparer.OrdinalIgnoreCase);
        ValidateRunOnlyCategories(options.RunOnlyCategories, normalizedRegisteredCategories, result);
        ValidateIgnoreCategories(options.IgnoreCategories, normalizedRegisteredCategories, result);
        return result;
    }

    private static IReadOnlySet<string> GetRegisteredCategories(IServiceProvider services)
    {
        var metadataRegistry = services.GetRequiredService<IModuleMetadataRegistry>();
        var registeredCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var module in services.GetServices<IModule>())
        {
            var moduleType = module.GetType();
            metadataRegistry.FinalizeMetadata(moduleType, module);
            if (metadataRegistry.GetCategory(moduleType) is { } category)
            {
                registeredCategories.Add(category);
            }
        }

        return registeredCategories;
    }

    private static void ValidateRunOnlyCategories(
        IReadOnlyList<string>? categories,
        IReadOnlySet<string> registeredCategories,
        ValidationResult result)
    {
        var configuredCategories = categories?
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray() ?? [];
        if (configuredCategories.Length == 0)
        {
            return;
        }

        var unmatchedCategories = configuredCategories
            .Where(category => !registeredCategories.Contains(category))
            .ToArray();

        if (unmatchedCategories.Length == configuredCategories.Length)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "RunOnlyCategories would select zero registered modules. " +
                $"No registered module matches: {string.Join(", ", unmatchedCategories)}"));
        }
        else if (unmatchedCategories.Length > 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "RunOnlyCategories contains categories with no registered modules: " +
                string.Join(", ", unmatchedCategories)));
        }
    }

    private static void ValidateIgnoreCategories(
        IReadOnlyList<string>? categories,
        IReadOnlySet<string> registeredCategories,
        ValidationResult result)
    {
        var unmatchedCategories = categories?
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Where(category => !registeredCategories.Contains(category))
            .ToArray() ?? [];
        if (unmatchedCategories.Length == 0)
        {
            return;
        }

        result.AddError(new ValidationError(
            ValidationErrorCategory.Options,
            "IgnoreCategories contains categories with no registered modules: " +
            string.Join(", ", unmatchedCategories)));
    }
}
