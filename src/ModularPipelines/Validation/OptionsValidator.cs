using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;

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

        if (options.AlwaysRunProgressTimeout < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"AlwaysRunProgressTimeout cannot be negative. Current value: {options.AlwaysRunProgressTimeout}"));
        }

        ValidateConsoleOptions(options.Console, result);
        ValidateRunReportOptions(options.RunReport, result);
        ValidateConcurrencyOptions(options.Concurrency, result);

        // Validate HTTP timeout if set
        if (options.Http.Timeout is { } httpTimeout && httpTimeout <= TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Http.Timeout must be positive. Current value: {httpTimeout}"));
        }

        ValidateHttpResilienceOptions(options.Http.Resilience, result);

        ValidateCategoryFilters(options, result);

        return result;
    }

    private static void ValidateConsoleOptions(
        PipelineConsoleOptions options,
        ValidationResult result)
    {
        if (options.ModuleOutputFlushInterval < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushInterval cannot be negative. Current value: {options.ModuleOutputFlushInterval}"));
        }
        else if (options.ModuleOutputFlushInterval > PipelineConsoleOptions.MaximumModuleOutputFlushInterval)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushInterval cannot exceed {PipelineConsoleOptions.MaximumModuleOutputFlushInterval}. " +
                $"Current value: {options.ModuleOutputFlushInterval}"));
        }

        if (options.ModuleOutputFlushThreshold < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Console.ModuleOutputFlushThreshold cannot be negative. Current value: {options.ModuleOutputFlushThreshold}"));
        }
    }

    private static void ValidateConcurrencyOptions(
        ConcurrencyOptions options,
        ValidationResult result)
    {
        if (options.MaxParallelism < 1)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.MaxParallelism must be at least 1. Current value: {options.MaxParallelism}"));
        }

        if (options.MaxCpuIntensiveModules is <= 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.MaxCpuIntensiveModules must be positive or null. Current value: {options.MaxCpuIntensiveModules}"));
        }

        if (options.MaxIoIntensiveModules is <= 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.MaxIoIntensiveModules must be positive or null. Current value: {options.MaxIoIntensiveModules}"));
        }

        if (options.NotificationTimeout < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.NotificationTimeout cannot be negative. Current value: {options.NotificationTimeout}"));
        }
        else if (options.NotificationTimeout.TotalMilliseconds > int.MaxValue)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Concurrency.NotificationTimeout cannot exceed {int.MaxValue} milliseconds. " +
                $"Current value: {options.NotificationTimeout}"));
        }
    }

    private static void ValidateCategoryFilters(
        PipelineOptions options,
        ValidationResult result)
    {
        if (options.RunOnlyCategories is null || options.IgnoreCategories is null)
        {
            return;
        }

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

        if (options.IncludeModuleOutput && options.MaxOutputBytesPerModule <= 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"RunReport.MaxOutputBytesPerModule must be positive. Current value: {options.MaxOutputBytesPerModule}"));
        }

        if (options.HistoryRetention > 0
            && string.IsNullOrWhiteSpace(options.HistoryDirectory))
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "RunReport.HistoryDirectory cannot be empty when run history is enabled."));
        }
    }

    private static void ValidateHttpResilienceOptions(
        HttpResilienceOptions? options,
        ValidationResult result)
    {
        if (options is null)
        {
            return;
        }

        if (options.MaxRetryAttempts < 0)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Http.Resilience.MaxRetryAttempts cannot be negative. Current value: {options.MaxRetryAttempts}"));
        }

        if (options.InitialDelay < TimeSpan.Zero || options.MaxDelay < TimeSpan.Zero)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "Http resilience delays cannot be negative."));
        }
        else if (options.InitialDelay > options.MaxDelay)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                "Http.Resilience.InitialDelay cannot exceed MaxDelay."));
        }

        if (options.JitterFactor is < 0 or > 1)
        {
            result.AddError(new ValidationError(
                ValidationErrorCategory.Options,
                $"Http.Resilience.JitterFactor must be between 0 and 1. Current value: {options.JitterFactor}"));
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
