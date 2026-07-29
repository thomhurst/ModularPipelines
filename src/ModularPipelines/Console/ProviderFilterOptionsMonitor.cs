using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal sealed class ProviderFilterOptionsMonitor(
    IOptionsMonitor<LoggerFilterOptions> inner,
    Type providerType) : IOptionsMonitor<LoggerFilterOptions>
{
    private static readonly string WrapperProviderName =
        typeof(NonOwningLoggerProvider).FullName!;

    private readonly string? _providerAlias =
        providerType.GetCustomAttribute<ProviderAliasAttribute>()?.Alias;

    private readonly string _providerName = providerType.FullName ?? providerType.Name;

    public LoggerFilterOptions CurrentValue => Translate(inner.CurrentValue);

    public LoggerFilterOptions Get(string? name) => Translate(inner.Get(name));

    public IDisposable? OnChange(Action<LoggerFilterOptions, string?> listener) =>
        inner.OnChange((options, name) => listener(Translate(options), name));

    private LoggerFilterOptions Translate(LoggerFilterOptions source)
    {
        var translated = new LoggerFilterOptions
        {
            CaptureScopes = source.CaptureScopes,
            MinLevel = source.MinLevel,
        };

        foreach (var rule in source.Rules)
        {
            translated.Rules.Add(Translate(rule));
        }

        return translated;
    }

    private LoggerFilterRule Translate(LoggerFilterRule rule)
    {
        var originalFilter = rule.Filter;
        var translatedProviderName = string.Equals(
                                         rule.ProviderName,
                                         _providerName,
                                         StringComparison.OrdinalIgnoreCase)
                                     || (_providerAlias is not null && string.Equals(
                                         rule.ProviderName,
                                         _providerAlias,
                                         StringComparison.OrdinalIgnoreCase))
            ? WrapperProviderName
            : rule.ProviderName;
        if (translatedProviderName == rule.ProviderName && originalFilter is null)
        {
            return rule;
        }

        return new LoggerFilterRule(
            translatedProviderName,
            rule.CategoryName,
            rule.LogLevel,
            originalFilter is null
                ? null
                : (_, categoryName, logLevel) => originalFilter(
                    _providerName,
                    categoryName,
                    logLevel));
    }
}
