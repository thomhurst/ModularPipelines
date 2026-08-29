using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal sealed class NoopSpectreConsoleLoggerControl(
    ILoggerFactory loggerFactory,
    IOptionsMonitor<LoggerFilterOptions> filterOptions,
    IEnumerable<ILoggerProvider> loggerProviders)
    : ISpectreConsoleLoggerControl
{
    private const string ConsoleProviderAlias = "Console";
    private static readonly string? ConsoleProviderName = typeof(ConsoleLoggerProvider).FullName;
    private readonly bool _hasConsoleProvider = loggerFactory.GetType() == typeof(LoggerFactory)
        && loggerProviders.Any(static provider => provider is ConsoleLoggerProvider);

    public object SynchronizationLock { get; } = new();

    public Task FlushAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public IDisposable Suspend() => NoopDisposable.Instance;

    public bool TryAcquireRenderGate(TimeSpan timeout, out IDisposable? gate)
    {
        gate = NoopDisposable.Instance;
        return true;
    }

    public ValueTask<IDisposable?> TryAcquireRenderGateAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default) =>
        ValueTask.FromResult<IDisposable?>(NoopDisposable.Instance);

    public bool WouldRender(string categoryName, LogLevel logLevel) =>
        _hasConsoleProvider && IsEnabled(filterOptions.CurrentValue, categoryName, logLevel);

    private static bool IsEnabled(
        LoggerFilterOptions options,
        string categoryName,
        LogLevel logLevel)
    {
        var selectedRule = SelectRule(options, categoryName);
        var minimumLevel = selectedRule is null ? options.MinLevel : selectedRule.LogLevel;
        if (minimumLevel is not null && logLevel < minimumLevel)
        {
            return false;
        }

        return selectedRule?.Filter?.Invoke(ConsoleProviderName, categoryName, logLevel) ?? true;
    }

    private static LoggerFilterRule? SelectRule(
        LoggerFilterOptions options,
        string categoryName)
    {
        // Match Microsoft.Extensions.Logging's internal LoggerRuleSelector so aliases,
        // provider precedence, category specificity, wildcards, and last-rule wins agree.
        LoggerFilterRule? selectedRule = null;
        foreach (var rule in options.Rules)
        {
            if (IsBetter(rule, selectedRule, ConsoleProviderName, categoryName)
                || IsBetter(rule, selectedRule, ConsoleProviderAlias, categoryName))
            {
                selectedRule = rule;
            }
        }

        return selectedRule;
    }

    private static bool IsBetter(
        LoggerFilterRule rule,
        LoggerFilterRule? selectedRule,
        string? providerName,
        string categoryName)
    {
        if (!MatchesProvider(rule.ProviderName, providerName)
            || !MatchesCategory(rule.CategoryName, categoryName))
        {
            return false;
        }

        var ruleTargetsProvider = rule.ProviderName is not null;
        var selectedRuleTargetsProvider = selectedRule?.ProviderName is not null;
        if (ruleTargetsProvider != selectedRuleTargetsProvider)
        {
            return ruleTargetsProvider;
        }

        return IsAtLeastAsSpecific(rule.CategoryName, selectedRule?.CategoryName);
    }

    private static bool MatchesProvider(string? ruleProviderName, string? providerName) =>
        ruleProviderName is null || ruleProviderName == providerName;

    private static bool IsAtLeastAsSpecific(string? ruleCategory, string? selectedCategory) =>
        selectedCategory is null
        || (ruleCategory is not null && selectedCategory.Length <= ruleCategory.Length);

    private static bool MatchesCategory(string? ruleCategory, string categoryName)
    {
        if (ruleCategory is null)
        {
            return true;
        }

        var wildcardIndex = ruleCategory.IndexOf('*');
        if (wildcardIndex >= 0 && ruleCategory.IndexOf('*', wildcardIndex + 1) >= 0)
        {
            throw new InvalidOperationException("Logger filter categories may contain at most one wildcard.");
        }

        var prefix = wildcardIndex < 0 ? ruleCategory : ruleCategory[..wildcardIndex];
        var suffix = wildcardIndex < 0 ? string.Empty : ruleCategory[(wildcardIndex + 1)..];
        return categoryName.Length >= prefix.Length + suffix.Length
               && categoryName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
               && categoryName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
    }

    private sealed class NoopDisposable : IDisposable
    {
        public static NoopDisposable Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
