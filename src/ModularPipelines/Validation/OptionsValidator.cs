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
    public Task<ValidationResult> ValidateAsync(IServiceProvider services)
    {
        var optionsSnapshot = services.GetService<IOptions<PipelineOptions>>();
        if (optionsSnapshot?.Value == null)
        {
            return Task.FromResult(ValidationResult.Success());
        }

        return Task.FromResult(ValidateOptions(
            optionsSnapshot.Value,
            GetRegisteredCategories(services)));
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

        var consoleOptions = options.Console;
        if (consoleOptions.ModuleOutputFlushInterval < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushInterval cannot be negative. Current value: {consoleOptions.ModuleOutputFlushInterval}"));
        }
        else if (consoleOptions.ModuleOutputFlushInterval > PipelineConsoleOptions.MaximumModuleOutputFlushInterval)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushInterval cannot exceed {PipelineConsoleOptions.MaximumModuleOutputFlushInterval}. " +
                $"Current value: {consoleOptions.ModuleOutputFlushInterval}"));
        }

        if (consoleOptions.ModuleOutputFlushThreshold < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushThreshold cannot be negative. Current value: {consoleOptions.ModuleOutputFlushThreshold}"));
        }

        ValidateRunReportOptions(options.RunReport, result);

        // Validate concurrency options
        if (options.Concurrency.MaxParallelism < 1)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.MaxParallelism must be at least 1. Current value: {options.Concurrency.MaxParallelism}"));
        }

        // Validate HTTP timeout if set
        if (options.Http.Timeout is { } httpTimeout && httpTimeout <= TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Http.Timeout must be positive. Current value: {httpTimeout}"));
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

    private static void ValidateRunReportOptions(
        RunReportOptions options,
        ValidationResult result)
    {
        if (options.HistoryRetention < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.HistoryRetention cannot be negative. Current value: {options.HistoryRetention}"));
        }

        if (options.GlobalHistoryRetention < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.GlobalHistoryRetention cannot be negative. Current value: {options.GlobalHistoryRetention}"));
        }

        if (options.HistoryRetention > 0
            && options.GlobalHistoryRetention > 0
            && options.GlobalHistoryRetention < options.HistoryRetention)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.GlobalHistoryRetention ({options.GlobalHistoryRetention}) cannot be lower than " +
                $"RunReport.HistoryRetention ({options.HistoryRetention})."));
        }

        if (options.HistoryRetention > 0
            && string.IsNullOrWhiteSpace(options.HistoryDirectory))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "RunReport.HistoryDirectory cannot be empty when run history is enabled."));
        }
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
