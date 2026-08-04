using Initialization.Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry;

internal sealed class OpenTelemetryInitializer : IInitializer, IDisposable
{
    private readonly TracerProvider _tracerProvider;
    private readonly MeterProvider _meterProvider;

    public OpenTelemetryInitializer(
        TracerProvider tracerProvider,
        MeterProvider meterProvider)
    {
        _tracerProvider = tracerProvider;
        _meterProvider = meterProvider;
    }

    public int Order => 0;

    public Task InitializeAsync() => Task.CompletedTask;

    public void Dispose()
    {
        _tracerProvider.ForceFlush();
        _meterProvider.ForceFlush();
    }
}
