using System.Reflection;
using Microsoft.Extensions.Logging;

namespace ModularPipelines.Console;

internal static class LoggerFilterRuleEvaluator
{
    public static bool IsEnabled(
        LoggerFilterOptions options,
        Type providerType,
        string categoryName,
        LogLevel logLevel)
    {
        var providerName = providerType.FullName;
        var providerAlias = providerType.GetCustomAttribute<ProviderAliasAttribute>()?.Alias;
        var selectedRule = SelectRule(options, providerName, providerAlias, categoryName);
        var minimumLevel = selectedRule is null ? options.MinLevel : selectedRule.LogLevel;
        if (minimumLevel is not null && logLevel < minimumLevel)
        {
            return false;
        }

        return selectedRule?.Filter?.Invoke(providerName, categoryName, logLevel) ?? true;
    }

    private static LoggerFilterRule? SelectRule(
        LoggerFilterOptions options,
        string? providerName,
        string? providerAlias,
        string categoryName)
    {
        // Match Microsoft.Extensions.Logging's internal LoggerRuleSelector so aliases,
        // provider precedence, category specificity, wildcards, and last-rule wins agree.
        LoggerFilterRule? selectedRule = null;
        foreach (var rule in options.Rules)
        {
            if (IsBetter(rule, selectedRule, providerName, categoryName)
                || IsBetter(rule, selectedRule, providerAlias, categoryName))
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
}
