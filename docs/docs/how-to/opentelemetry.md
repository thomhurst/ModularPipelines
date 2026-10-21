---
title: OpenTelemetry
sidebar_position: 24
---

# OpenTelemetry

Modular Pipelines emits traces and metrics through the standard .NET
`ActivitySource` and `Meter` APIs. Add the optional integration package to connect
that instrumentation to an OpenTelemetry collector:

```shell
dotnet add package ModularPipelines.OpenTelemetry
```

Register the instrumentation and the OTLP exporter when creating the pipeline:

```csharp
using ModularPipelines;
using ModularPipelines.OpenTelemetry;

var builder = Pipeline.CreateBuilder();

builder.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter());
```

The unified exporter sends traces, metrics, and `ILogger` logs, including logs
from categories outside Modular Pipelines. OpenTelemetry log export does not apply
module secret masking: filter or redact sensitive log messages before export. Use
`openTelemetry.Builder.WithLogging(...)` for OpenTelemetry logging configuration
and the standard `builder.Services.AddLogging(...)` APIs for category and provider
filters.

The exporter uses the standard `OTEL_EXPORTER_OTLP_*` environment variables. For
example, set `OTEL_EXPORTER_OTLP_ENDPOINT` to the collector endpoint.

The trace and metric providers start when the pipeline is built. When the pipeline
is disposed, both providers are flushed before their exporters are shut down.

## Exporter configuration

Pass an options transformation to configure the OTLP exporter in code. Return the
supplied options instance or a replacement:

```csharp
using OpenTelemetry.Exporter;

builder.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter(options =>
{
    options.Endpoint = new Uri("https://collector.example.com/otlp/");
    options.Headers = "x-api-key=your-token";
    options.Protocol = OtlpExportProtocol.HttpProtobuf;
    return options;
}));
```

These are common options for all signals. With HTTP/protobuf, set a base endpoint;
the exporter appends `v1/traces`, `v1/metrics`, and `v1/logs`. Signal-specific
environment variables, such as `OTEL_EXPORTER_OTLP_TRACES_ENDPOINT`, take precedence
over common options.

Use `ConfigureTracing` and `ConfigureMetrics` to add instrumentation, processors,
readers, or exporters through the standard OpenTelemetry builders. These hooks
retain the pipeline's default sources and meter:

```csharp
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

builder.AddOpenTelemetry(openTelemetry => openTelemetry
    .ConfigureTracing(tracing => tracing.AddSource("MyApplication"))
    .ConfigureMetrics(metrics => metrics.AddMeter("MyApplication")));
```

The hooks accept `Action` callbacks, matching the mutable OpenTelemetry builder
APIs. `openTelemetry.Builder` also exposes `IOpenTelemetryBuilder` for resource,
logging, or other OpenTelemetry configuration. The existing parameterless
`AddOtlpExporter()` remains available. Repeated calls to either overload register
the unified exporter once per service collection, including across separate
`AddOpenTelemetry` callbacks. Options transformations compose in registration
order. OpenTelemetry's unified exporter cannot be combined with signal-specific
`AddOtlpExporter` registrations or another direct `UseOtlpExporter` call.

## Migrating to V4

`OpenTelemetryPipelineBuilderExtensions` now lives in
`ModularPipelines.OpenTelemetry`. Replace the `ModularPipelines.Extensions` import
used for `builder.AddOpenTelemetry(...)` with `ModularPipelines.OpenTelemetry`.
Keep other imports if your code uses unrelated extensions from them.

For static calls, change
`ModularPipelines.Extensions.OpenTelemetryPipelineBuilderExtensions.AddOpenTelemetry(builder)`
to
`ModularPipelines.OpenTelemetry.OpenTelemetryPipelineBuilderExtensions.AddOpenTelemetry(builder)`.
Rebuild callers against V4. The previous namespace has no forwarding type.
Registration, exporter configuration, provider flushing, and telemetry behavior
are unchanged by this namespace move.

V4 also leaves nonfailing span status unchanged (normally `Unset`) instead of
setting `Ok`. Update filters that require `Ok` for successful operations. Module
metrics no longer include `modular_pipelines.module.type_full`; use
`modular_pipelines.module.type` in metric queries. Full type names remain on module
spans.

## Traces

Each run creates a `Pipeline.Run` root span. Module spans are its children, and
commands executed by a module create command child spans. Command spans include the
tool and exit code. Read duration from the span itself. Arguments use the same secret obfuscation as command
logs, so registered secrets are not exported.

The integration subscribes to these activity sources:

- `ModularPipelines`
- `ModularPipelines.Modules`
- `ModularPipelines.Commands`

The activity sources and meter use the core assembly version as their
instrumentation version.

Failures include an `exception` event with the original `exception.type`, a masked
`exception.message`, and an `exception.stacktrace` representation of a newly
constructed exception containing only the masked message. The original exception
object and its stack trace are not exported. An `error.type` span attribute also
identifies the original exception type.

Failed, timed-out, and canceled operations set the span status to `Error`.
Successful operations, skipped modules, restored results, and ignored failures
leave the span status unchanged (normally `Unset`); they do not explicitly set
`Ok`. Ignored failures still include the sanitized exception event and `error.type`
but do not increment the failed-module counter. Pipeline and module status
attributes use lowercase snake case, such as `succeeded`, `failed`, `timed_out`,
and `restored_from_cache`.

Module spans include `modular_pipelines.module.cache`: `hit` when the result is
restored from the fingerprint cache, `miss` when a lookup finds no usable result,
and `disabled` when module caching is disabled or no cache store is configured.
Skipped modules do not perform a cache lookup.

## Metrics

The `ModularPipelines` meter publishes:

- `modular_pipelines.module.duration` — module duration in seconds
- `modular_pipelines.module.failed` — failed module count
- `modular_pipelines.module.retries` — module retry count
- `modular_pipelines.module.cache_hits` — fingerprint cache hit count
- `modular_pipelines.module.cache_misses` — fingerprint cache miss count

Module metrics include the short type name in `modular_pipelines.module.type`.
The full type name, `modular_pipelines.module.type_full`, appears only on module
spans to avoid high-cardinality metric dimensions, including versioned generic
type arguments. Duration and failed-module metrics also include
`modular_pipelines.module.status`. Cache counters increment once per lookup
outcome; disabled caching does not increment either counter.

## Resource attributes

The pipeline application name becomes the OpenTelemetry service name. On common CI
providers, the integration also maps the commit environment variable (for example,
`GITHUB_SHA` or `CI_COMMIT_SHA`) to `vcs.ref.head.revision`.

Exporter registration is optional. Without `AddOtlpExporter`, you can add another
exporter through `ConfigureTracing` and `ConfigureMetrics`, or through the normal
OpenTelemetry services.
