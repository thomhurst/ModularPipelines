# OpenTelemetry

Modular Pipelines emits traces and metrics through the standard .NET `ActivitySource` and `Meter` APIs. Add the optional integration package to connect that instrumentation to an OpenTelemetry collector:

```
dotnet add package ModularPipelines.OpenTelemetry
```

Register the instrumentation and the OTLP exporter when creating the pipeline:

```
using ModularPipelines;

using ModularPipelines.OpenTelemetry;



var builder = Pipeline.CreateBuilder();



builder.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter());
```

The exporter uses the standard `OTEL_EXPORTER_OTLP_*` environment variables. For example, set `OTEL_EXPORTER_OTLP_ENDPOINT` to the collector endpoint.

The trace and metric providers start when the pipeline is built. When the pipeline is disposed, both providers are flushed before their exporters are shut down.

## Exporter configuration[​](#exporter-configuration "Direct link to Exporter configuration")

Pass an options transformation to configure the OTLP exporter in code. Return the supplied options instance or a replacement:

```
using OpenTelemetry.Exporter;



builder.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter(options =>

{

    options.Endpoint = new Uri("https://collector.example.com/otlp/");

    options.Headers = "x-api-key=your-token";

    options.Protocol = OtlpExportProtocol.HttpProtobuf;

    return options;

}));
```

These are common options for all signals. With HTTP/protobuf, set a base endpoint; the exporter appends paths such as `v1/traces` and `v1/metrics`. Signal-specific environment variables, such as `OTEL_EXPORTER_OTLP_TRACES_ENDPOINT`, take precedence over common options.

Use `ConfigureTracing` and `ConfigureMetrics` to add instrumentation, processors, readers, or exporters through the standard OpenTelemetry builders. These hooks retain the pipeline's default sources and meter:

```
using OpenTelemetry.Metrics;

using OpenTelemetry.Trace;



builder.AddOpenTelemetry(openTelemetry => openTelemetry

    .ConfigureTracing(tracing => tracing.AddSource("MyApplication"))

    .ConfigureMetrics(metrics => metrics.AddMeter("MyApplication")));
```

The hooks accept `Action` callbacks, matching the mutable OpenTelemetry builder APIs. `openTelemetry.Builder` also exposes `IOpenTelemetryBuilder` for resource, logging, or other OpenTelemetry configuration. The existing parameterless `AddOtlpExporter()` remains available. Call either exporter overload only once; OpenTelemetry's unified exporter cannot be combined with signal-specific `AddOtlpExporter` registrations.

## Migrating to V4[​](#migrating-to-v4 "Direct link to Migrating to V4")

`OpenTelemetryPipelineBuilderExtensions` now lives in `ModularPipelines.OpenTelemetry`. Replace the `ModularPipelines.Extensions` import used for `builder.AddOpenTelemetry(...)` with `ModularPipelines.OpenTelemetry`. Keep other imports if your code uses unrelated extensions from them.

For static calls, change `ModularPipelines.Extensions.OpenTelemetryPipelineBuilderExtensions.AddOpenTelemetry(builder)` to `ModularPipelines.OpenTelemetry.OpenTelemetryPipelineBuilderExtensions.AddOpenTelemetry(builder)`. Rebuild callers against V4. The previous namespace has no forwarding type. Registration, exporter configuration, provider flushing, and telemetry behavior are unchanged by this namespace move.

## Traces[​](#traces "Direct link to Traces")

Each run creates a `Pipeline.Run` root span. Module spans are its children, and commands executed by a module create command child spans. Command spans include the tool and exit code. Read duration from the span itself. Arguments use the same secret obfuscation as command logs, so registered secrets are not exported.

The integration subscribes to these activity sources:

* `ModularPipelines`
* `ModularPipelines.Modules`
* `ModularPipelines.Commands`

Failures include an `exception` event with `exception.type` and a masked `exception.message`, plus an `error.type` span attribute. Raw exception objects and stack traces are not exported. Ignored failures include the same sanitized event and `error.type`, but keep an `Ok` span status and do not increment the failed-module counter. Pipeline and module status attributes use lowercase snake case, such as `succeeded`, `failed`, `timed_out`, and `restored_from_cache`.

Module spans include `modular_pipelines.module.cache`: `hit` when the result is restored from the fingerprint cache, `miss` when a lookup finds no usable result, and `disabled` when module caching is disabled or no cache store is configured. Skipped modules do not perform a cache lookup.

## Metrics[​](#metrics "Direct link to Metrics")

The `ModularPipelines` meter publishes:

* `modular_pipelines.module.duration` — module duration in seconds
* `modular_pipelines.module.failed` — failed module count
* `modular_pipelines.module.retries` — module retry count
* `modular_pipelines.module.cache_hits` — fingerprint cache hit count
* `modular_pipelines.module.cache_misses` — fingerprint cache miss count

Module metrics include `modular_pipelines.module.type` and `modular_pipelines.module.type_full` attributes. Duration and failed-module metrics also include `modular_pipelines.module.status`. Cache counters increment once per lookup outcome; disabled caching does not increment either counter.

## Resource attributes[​](#resource-attributes "Direct link to Resource attributes")

The pipeline application name becomes the OpenTelemetry service name. On common CI providers, the integration also maps the commit environment variable (for example, `GITHUB_SHA` or `CI_COMMIT_SHA`) to `vcs.ref.head.revision`.

Exporter registration is optional. Without `AddOtlpExporter`, you can add another exporter through `ConfigureTracing` and `ConfigureMetrics`, or through the normal OpenTelemetry services.
