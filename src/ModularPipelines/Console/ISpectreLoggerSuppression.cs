using MEL.Spectre;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Console;

internal interface ISpectreLoggerSuppression
{
    bool IsSuppressed { get; }

    IDisposable BeginSuppression();
}

internal sealed class SpectreLoggerSuppression : ISpectreLoggerSuppression
{
    private readonly AsyncLocal<int> _suppressionDepth = new();

    public bool IsSuppressed => _suppressionDepth.Value > 0;

    public IDisposable BeginSuppression()
    {
        _suppressionDepth.Value++;
        return new SuppressionScope(this);
    }

    private sealed class SuppressionScope(SpectreLoggerSuppression owner) : IDisposable
    {
        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            owner._suppressionDepth.Value--;
            _isDisposed = true;
        }
    }
}

[ProviderAlias(SpectreLoggerSuppressionRegistration.SpectreProviderAlias)]
internal sealed class SuppressibleSpectreLoggerProvider(
    ILoggerProvider inner,
    ISpectreConsoleLoggerControl control,
    ISpectreLoggerSuppression suppression)
    : ILoggerProvider, ISupportExternalScope, ISpectreConsoleLoggerControl, IAsyncDisposable
{
    private int _isDisposed;

    public object SynchronizationLock => control.SynchronizationLock;

    public ILogger CreateLogger(string categoryName) =>
        new SuppressibleSpectreLogger(inner.CreateLogger(categoryName), suppression);

    public Task FlushAsync(CancellationToken cancellationToken = default) =>
        control.FlushAsync(cancellationToken);

    public void SetScopeProvider(IExternalScopeProvider scopeProvider)
    {
        if (inner is ISupportExternalScope supportExternalScope)
        {
            supportExternalScope.SetScopeProvider(scopeProvider);
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) == 0)
        {
            inner.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Interlocked.Exchange(ref _isDisposed, 1) != 0)
        {
            return;
        }

        if (inner is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            inner.Dispose();
        }
    }

    private sealed class SuppressibleSpectreLogger(
        ILogger inner,
        ISpectreLoggerSuppression suppression) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => inner.BeginScope(state);

        public bool IsEnabled(LogLevel logLevel) =>
            !suppression.IsSuppressed && inner.IsEnabled(logLevel);

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!suppression.IsSuppressed)
            {
                inner.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}

internal static class SpectreLoggerSuppressionRegistration
{
    internal const string SpectreProviderAlias = "SpectreConsole";
    internal const string SpectreProviderTypeName = "MEL.Spectre.Provider.SpectreConsoleLoggerProvider";

    public static void MakeSpectreLoggerSuppressible(this IServiceCollection services)
    {
        var providerDescriptor = services.LastOrDefault(static service =>
            service.ServiceType == typeof(ILoggerProvider)
            && service.ImplementationType?.FullName is SpectreProviderTypeName);
        if (providerDescriptor?.ImplementationType is not { } providerType)
        {
            throw new InvalidOperationException("The MEL.Spectre logger provider is not registered.");
        }

        var controlDescriptor = services.LastOrDefault(static service =>
            service.ServiceType == typeof(ISpectreConsoleLoggerControl));
        if (controlDescriptor?.ImplementationType is not { } controlType)
        {
            throw new InvalidOperationException("The MEL.Spectre logger control is not registered.");
        }

        services.Remove(providerDescriptor);
        services.Remove(controlDescriptor);
        services.TryAddSingleton<ISpectreLoggerSuppression, SpectreLoggerSuppression>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<
                IPostConfigureOptions<LoggerFilterOptions>,
                SpectreLoggerFilterOptionsPostConfigure>());
        services.AddSingleton(serviceProvider =>
        {
            var provider = (ILoggerProvider) ActivatorUtilities.CreateInstance(
                serviceProvider,
                providerType);
            ILoggerProvider[] controlProviders = [provider];
            var control = (ISpectreConsoleLoggerControl) ActivatorUtilities.CreateInstance(
                serviceProvider,
                controlType,
                (object) controlProviders);
            return new SuppressibleSpectreLoggerProvider(
                provider,
                control,
                serviceProvider.GetRequiredService<ISpectreLoggerSuppression>());
        });
        services.AddSingleton<ILoggerProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<SuppressibleSpectreLoggerProvider>());
        services.AddSingleton<ISpectreConsoleLoggerControl>(serviceProvider =>
            serviceProvider.GetRequiredService<SuppressibleSpectreLoggerProvider>());
        services.MakeLoggerFactoryTrackProviders();
    }

    private static void MakeLoggerFactoryTrackProviders(this IServiceCollection services)
    {
        var factoryDescriptor = services.LastOrDefault(static service =>
            service.ServiceType == typeof(ILoggerFactory));
        if (factoryDescriptor is null)
        {
            throw new InvalidOperationException("The logger factory is not registered.");
        }

        services.Remove(factoryDescriptor);
        services.AddSingleton(serviceProvider =>
        {
            var inner = CreateLoggerFactory(serviceProvider, factoryDescriptor);
            return new ProviderTrackingLoggerFactory(
                inner,
                serviceProvider.GetServices<ILoggerProvider>(),
                disposeInner: factoryDescriptor.ImplementationInstance is null);
        });
        services.AddSingleton<ILoggerFactory>(serviceProvider =>
            serviceProvider.GetRequiredService<ProviderTrackingLoggerFactory>());
        services.AddSingleton<ILoggerProviderRegistry>(serviceProvider =>
            serviceProvider.GetRequiredService<ProviderTrackingLoggerFactory>());
    }

    private static ILoggerFactory CreateLoggerFactory(
        IServiceProvider serviceProvider,
        ServiceDescriptor descriptor)
    {
        if (descriptor.ImplementationInstance is ILoggerFactory instance)
        {
            return instance;
        }

        if (descriptor.ImplementationFactory is not null)
        {
            return (ILoggerFactory) descriptor.ImplementationFactory(serviceProvider);
        }

        if (descriptor.ImplementationType is not null)
        {
            return (ILoggerFactory) ActivatorUtilities.CreateInstance(
                serviceProvider,
                descriptor.ImplementationType);
        }

        throw new InvalidOperationException("The logger factory registration is unsupported.");
    }
}

internal sealed class SpectreLoggerFilterOptionsPostConfigure
    : IPostConfigureOptions<LoggerFilterOptions>
{
    public void PostConfigure(string? name, LoggerFilterOptions options)
    {
        for (var index = 0; index < options.Rules.Count; index++)
        {
            var rule = options.Rules[index];
            var translatedRule = TranslateRule(rule);
            if (!ReferenceEquals(translatedRule, rule))
            {
                options.Rules[index] = translatedRule;
            }
        }
    }

    private static LoggerFilterRule TranslateRule(LoggerFilterRule rule)
    {
        if (!TargetsSpectreProvider(rule.ProviderName))
        {
            return rule;
        }

        var providerName = string.Equals(
            rule.ProviderName,
            SpectreLoggerSuppressionRegistration.SpectreProviderTypeName,
            StringComparison.OrdinalIgnoreCase)
            ? typeof(SuppressibleSpectreLoggerProvider).FullName
            : rule.ProviderName;
        var filter = TranslateFilter(rule.Filter);
        if (providerName == rule.ProviderName && ReferenceEquals(filter, rule.Filter))
        {
            return rule;
        }

        return new LoggerFilterRule(
            providerName,
            rule.CategoryName,
            rule.LogLevel,
            filter);
    }

    private static bool TargetsSpectreProvider(string? providerName) =>
        providerName is null
        || string.Equals(
            providerName,
            SpectreLoggerSuppressionRegistration.SpectreProviderAlias,
            StringComparison.OrdinalIgnoreCase)
        || string.Equals(
            providerName,
            SpectreLoggerSuppressionRegistration.SpectreProviderTypeName,
            StringComparison.OrdinalIgnoreCase);

    private static Func<string?, string?, LogLevel, bool>? TranslateFilter(
        Func<string?, string?, LogLevel, bool>? filter)
    {
        if (filter is null)
        {
            return null;
        }

        return (providerName, categoryName, logLevel) => filter(
            providerName == typeof(SuppressibleSpectreLoggerProvider).FullName
                ? SpectreLoggerSuppressionRegistration.SpectreProviderTypeName
                : providerName,
            categoryName,
            logLevel);
    }
}
