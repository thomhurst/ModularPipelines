using Initialization.Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace ModularPipelines.OpenTelemetry;

// IInitializer forces the providers to be constructed during pipeline startup. Because this
// singleton is constructed after its provider dependencies, DI disposes it first so both
// providers can be flushed before their exporters are shut down.
internal sealed class OpenTelemetryInitializer : IInitializer, IDisposable
{
    private const int FlushTimeoutMilliseconds = 5_000;

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
        try
        {
            _tracerProvider.ForceFlush(FlushTimeoutMilliseconds);
        }
        finally
        {
            _meterProvider.ForceFlush(FlushTimeoutMilliseconds);
        }
    }
}
