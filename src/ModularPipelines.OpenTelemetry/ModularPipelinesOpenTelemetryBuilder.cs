using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

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
    /// Gets the underlying builder for additional OpenTelemetry configuration.
    /// </summary>
    public IOpenTelemetryBuilder Builder => _builder;

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

    /// <summary>
    /// Adds the unified OTLP exporter with common exporter options for all signals.
    /// </summary>
    /// <param name="configure">Transforms the exporter options. May return the supplied instance or a replacement.</param>
    /// <returns>This builder.</returns>
    /// <remarks>
    /// For HTTP/protobuf, the endpoint is a base URL; signal-specific paths are appended automatically.
    /// Signal-specific OTEL_EXPORTER_OTLP_* environment variables take precedence over these common options.
    /// </remarks>
    public ModularPipelinesOpenTelemetryBuilder AddOtlpExporter(Func<OtlpExporterOptions, OtlpExporterOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        _builder.UseOtlpExporter();
        _builder.Services.Configure<OtlpExporterOptions>(options =>
        {
            var configured = configure(options)
                ?? throw new InvalidOperationException("The OTLP exporter options configuration returned null.");
            if (ReferenceEquals(configured, options))
            {
                return;
            }

            // OpenTelemetry owns the mutable options instance. Copy a returned replacement into it
            // so its options factory, named configuration, and post-configuration remain in effect.
            options.Endpoint = configured.Endpoint;
            options.Headers = configured.Headers;
            options.TimeoutMilliseconds = configured.TimeoutMilliseconds;
            options.Protocol = configured.Protocol;
            options.Compression = configured.Compression;
            options.MaxRequestSizeBytes = configured.MaxRequestSizeBytes;
            options.MaxResponseSizeBytes = configured.MaxResponseSizeBytes;
            options.UserAgentProductIdentifier = configured.UserAgentProductIdentifier;
            options.ExportProcessorType = configured.ExportProcessorType;
            options.BatchExportProcessorOptions = configured.BatchExportProcessorOptions;
            options.HttpClientFactory = configured.HttpClientFactory;
        });
        return this;
    }

    /// <summary>
    /// Configures tracing, including additional instrumentation, processors, and exporters.
    /// </summary>
    /// <param name="configure">Configures the underlying tracer provider builder.</param>
    /// <returns>This builder.</returns>
    public ModularPipelinesOpenTelemetryBuilder ConfigureTracing(Action<TracerProviderBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        _builder.WithTracing(configure);
        return this;
    }

    /// <summary>
    /// Configures metrics, including additional instrumentation, readers, and exporters.
    /// </summary>
    /// <param name="configure">Configures the underlying meter provider builder.</param>
    /// <returns>This builder.</returns>
    public ModularPipelinesOpenTelemetryBuilder ConfigureMetrics(Action<MeterProviderBuilder> configure)
    {
        ArgumentNullException.ThrowIfNull(configure);

        _builder.WithMetrics(configure);
        return this;
    }
}
