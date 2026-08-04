---
title: Run reports and history
---

ModularPipelines can write a schema-versioned JSON report after every pipeline run. Reports contain
pipeline and module statuses, timings, skip reasons, exception details, command counts, execution
metrics, duration changes from the previous retained run, and correlation metadata.

## Write a report

Configure an explicit output path on the pipeline builder:

```csharp
using ModularPipelines.Extensions;

var builder = Pipeline.CreateBuilder(args);
builder.WriteRunReport("artifacts/run-report.json");
```

Known CI systems automatically write `artifacts/run-report.json` when no explicit path is set. To
disable that behavior while keeping an explicitly configured path available, set `AutoWriteInCi`:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    RunReport = options.RunReport with
    {
        AutoWriteInCi = false,
    },
});
```

The current schema version is available as `PipelineRunReport.CurrentSchemaVersion`. The completed
report is also exposed through `PipelineSummary.RunReport`.

Each schema-v2 report has a unique `RunId` plus `RunCorrelation` metadata for the machine and
detected build system. Registering the Git or GitHub integration also adds the available commit,
branch, and CI run URL. Correlation strings pass through secret obfuscation before persistence.

When report writing is enabled, add application-specific metadata through a bounded
`IRunReportEnricher`:

```csharp
public sealed class DeploymentRunEnricher : IRunReportEnricher
{
    public ValueTask EnrichAsync(
        RunReportEnrichmentContext context,
        CancellationToken cancellationToken)
    {
        context.GitBranch ??= "deployment";
        return ValueTask.CompletedTask;
    }
}

builder.AddRunReportEnricher<DeploymentRunEnricher>();
```

Enrichers run sequentially in registration order. Use `??=` for fallback metadata so an earlier
value survives. Overwrite an existing value only when the current source is authoritative; later
authoritative enrichers take precedence. The built-in Git enricher fills gaps, while the GitHub
enricher replaces Git values with CI-provided commit and branch metadata when available.

## Local history and deltas

When report writing is enabled, the default `IRunHistoryStore` saves reports under
`.modularpipelines/run-history`. It retains the latest 20 reports and uses the newest compatible
report to calculate module and total-duration deltas. When a previous duration exists, the final
results table includes a `Δ previous` column.

Configure or disable retention with `RunReportOptions`:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    RunReport = options.RunReport with
    {
        HistoryDirectory = "artifacts/run-history",
        HistoryRetention = 10, // Use 0 to disable history.
        PipelineIdentity = "release-pipeline",
    },
});
```

History is partitioned by pipeline identity and pruning only removes files owned by the built-in
history store. When `PipelineIdentity` is omitted, Modular Pipelines derives one from the report
path and registered module types. History is bounded: after each save, the default store deletes
owned files beyond the configured limit for that pipeline.
Report and history I/O failures are logged as warnings and do not replace a pipeline failure.

## Custom history stores

Implement `IRunHistoryStore` to keep reports in a database, object store, or another backend, then
register it on the builder:

```csharp
builder.AddRunHistoryStore<MyRunHistoryStore>();
```

The store returns the latest report for the requested pipeline identity and saves the completed
current report. Custom stores own their retention behavior.

In v4, `IRunHistoryStore` removes the parameterless `GetLatestAsync` member. Existing custom stores
must move any identity filtering into `GetLatestAsync(string pipelineIdentity, CancellationToken)`,
which is now the only read member to implement.
