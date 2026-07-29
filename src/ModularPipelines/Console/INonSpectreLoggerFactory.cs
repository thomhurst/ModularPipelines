using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface INonSpectreLoggerFactory
{
    IReadOnlyList<ILogger> CreateLoggers(string categoryName);
}

internal sealed class NonSpectreLoggerFactory(
    IEnumerable<ILoggerProvider> providers,
    IOptionsMonitor<LoggerFilterOptions> filterOptions) : INonSpectreLoggerFactory
{
    private const string SpectreProviderTypeName = "MEL.Spectre.Provider.SpectreConsoleLoggerProvider";
    private readonly ConcurrentDictionary<string, IReadOnlyList<ILogger>> _loggers = new();
    private readonly ILoggerProvider[] _providers =
    [
        .. providers.Where(static provider =>
            provider.GetType().FullName is not SpectreProviderTypeName),
    ];

    public IReadOnlyList<ILogger> CreateLoggers(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, CreateLoggersCore);
    }

    private IReadOnlyList<ILogger> CreateLoggersCore(string categoryName)
    {
        return
        [
            .. _providers.Select(provider => new FilteredProviderLogger(
                provider.CreateLogger(categoryName),
                provider.GetType(),
                categoryName,
                filterOptions)),
        ];
    }

    private sealed class FilteredProviderLogger(
        ILogger inner,
        Type providerType,
        string categoryName,
        IOptionsMonitor<LoggerFilterOptions> filterOptions) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel)
        {
            var options = filterOptions.CurrentValue;
            var rule = SelectRule(options, providerType, categoryName);
            var minimumLevel = rule is null ? options.MinLevel : rule.LogLevel;
            if (minimumLevel is { } level && logLevel < level)
            {
                return false;
            }

            return (rule?.Filter?.Invoke(providerType.FullName, categoryName, logLevel) ?? true)
                   && inner.IsEnabled(logLevel);
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                inner.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }

    private static LoggerFilterRule? SelectRule(
        LoggerFilterOptions options,
        Type providerType,
        string categoryName)
    {
        var providerName = providerType.FullName;
        var providerAlias = providerType.GetCustomAttribute<ProviderAliasAttribute>()?.Alias;
        LoggerFilterRule? current = null;

        foreach (var rule in options.Rules)
        {
            if (IsBetter(rule, current, providerName, categoryName)
                || (!string.IsNullOrEmpty(providerAlias)
                    && IsBetter(rule, current, providerAlias, categoryName)))
            {
                current = rule;
            }
        }

        return current;
    }

    private static bool IsBetter(
        LoggerFilterRule rule,
        LoggerFilterRule? current,
        string? providerName,
        string categoryName)
    {
        if (rule.ProviderName is not null && rule.ProviderName != providerName)
        {
            return false;
        }

        if (!CategoryMatches(rule.CategoryName, categoryName))
        {
            return false;
        }

        if (current is null)
        {
            return true;
        }

        if (current.ProviderName is not null && rule.ProviderName is null)
        {
            return false;
        }

        if (current.ProviderName is null && rule.ProviderName is not null)
        {
            return true;
        }

        if (current.CategoryName is null)
        {
            return true;
        }

        if (rule.CategoryName is null)
        {
            return false;
        }

        return current.CategoryName.Length <= rule.CategoryName.Length;
    }

    private static bool CategoryMatches(string? ruleCategory, string categoryName)
    {
        if (ruleCategory is null)
        {
            return true;
        }

        var wildcardIndex = ruleCategory.IndexOf('*');
        if (wildcardIndex >= 0 && ruleCategory.IndexOf('*', wildcardIndex + 1) >= 0)
        {
            throw new InvalidOperationException("Logger filter categories cannot contain more than one wildcard.");
        }

        var prefix = wildcardIndex < 0 ? ruleCategory : ruleCategory[..wildcardIndex];
        var suffix = wildcardIndex < 0 ? string.Empty : ruleCategory[(wildcardIndex + 1)..];
        return categoryName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
               && categoryName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
    }
}
