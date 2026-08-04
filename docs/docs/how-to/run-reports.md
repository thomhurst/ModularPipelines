---
title: Run reports and history
---

ModularPipelines can write a schema-versioned JSON report after every pipeline run. Reports contain
pipeline and module statuses, timings, skip reasons, exception details, command counts, execution
metrics, and duration changes from the previous retained run.

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

The store returns the latest report for comparison and saves the completed current report. Custom
stores own their retention behavior. Override the identity-aware `GetLatestAsync` overload when the
backend can search past a newer report belonging to another pipeline; its default implementation
only validates the store's latest report.
