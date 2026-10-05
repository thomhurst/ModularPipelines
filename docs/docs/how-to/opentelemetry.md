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
using ModularPipelines.Extensions;

var builder = Pipeline.CreateBuilder();

builder.AddOpenTelemetry(openTelemetry => openTelemetry.AddOtlpExporter());
```

The exporter uses the standard `OTEL_EXPORTER_OTLP_*` environment variables. For
example, set `OTEL_EXPORTER_OTLP_ENDPOINT` to the collector endpoint.

The trace and metric providers start when the pipeline is built. When the pipeline
is disposed, both providers are flushed before their exporters are shut down.

## Traces

Each run creates a `Pipeline.Run` root span. Module spans are its children, and
commands executed by a module create command child spans. Command spans include the
tool and exit code. Read duration from the span itself. Arguments use the same secret obfuscation as command
logs, so registered secrets are not exported.

The integration subscribes to these activity sources:

- `ModularPipelines`
- `ModularPipelines.Modules`
- `ModularPipelines.Commands`

Failures include an `exception` event with `exception.type` and a masked
`exception.message`, plus an `error.type` span attribute. Raw exception objects and
stack traces are not exported. Ignored failures include the same sanitized event and
`error.type`, but keep an `Ok` span status and do not increment the failed-module
counter. Pipeline and module status attributes use lowercase
snake case, such as `succeeded`, `failed`, `timed_out`, and `restored_from_cache`.

## Metrics

The `ModularPipelines` meter publishes:

- `modular_pipelines.module.duration` — module duration in seconds
- `modular_pipelines.module.failed` — failed module count
- `modular_pipelines.module.retries` — module retry count

Module metrics include module type and status attributes.

## Resource attributes

The pipeline application name becomes the OpenTelemetry service name. On common CI
providers, the integration also maps the commit environment variable (for example,
`GITHUB_SHA` or `CI_COMMIT_SHA`) to `vcs.ref.head.revision`.

Exporter registration is optional. Without `AddOtlpExporter`, you can add another
exporter by registering it through the normal OpenTelemetry services.
