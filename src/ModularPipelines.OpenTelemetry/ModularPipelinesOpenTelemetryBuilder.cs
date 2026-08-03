using OpenTelemetry;

namespace ModularPipelines.OpenTelemetry;

/// <summary>
/// Configures OpenTelemetry export for Modular Pipelines instrumentation.
/// </summary>
public sealed class ModularPipelinesOpenTelemetryBuilder
{
    private readonly IOpenTelemetryBuilder _builder;

    internal ModularPipelinesOpenTelemetryBuilder(IOpenTelemetryBuilder builder)
    {
        _builder = builder;
    }

    /// <summary>
    /// Adds the unified OTLP exporter for traces and metrics.
    /// </summary>
    /// <returns>This builder.</returns>
    /// <remarks>
    /// Configure the exporter through the standard OTEL_EXPORTER_OTLP_* environment variables.
    /// </remarks>
    public ModularPipelinesOpenTelemetryBuilder AddOtlpExporter()
    {
        _builder.UseOtlpExporter();
        return this;
    }
}
