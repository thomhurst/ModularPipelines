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

By default, the `IRunHistoryStore` saves reports under `.modularpipelines/run-history` on local and
CI runs, even when JSON report writing is disabled. It retains the latest 20 reports and uses the
newest compatible report to calculate module and total-duration deltas. When a previous duration
exists, the final results table includes a `Δ previous` column. Deltas compare only successful
runs and successful module executions, so failed or timed-out durations do not create false
regressions on a later run.

Add the default history directory to `.gitignore` if you do not want to commit local run data:

```gitignore
.modularpipelines/run-history/
```

Configure or disable retention with `RunReportOptions`:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    RunReport = options.RunReport with
    {
        HistoryDirectory = "artifacts/run-history",
        HistoryRetention = 10, // Use 0 to disable history.
        GlobalHistoryRetention = 100, // Use 0 for no global limit.
        PipelineIdentity = "release-pipeline",
    },
});
```

History is partitioned by pipeline identity and pruning only removes files owned by the built-in
history store. When `PipelineIdentity` is omitted, Modular Pipelines derives one from the report
path and registered module types. After each save, the default store applies the per-identity
`HistoryRetention` limit, then keeps the newest `GlobalHistoryRetention` reports across all
identities. The global limit supersedes the per-identity limit: a quieter identity can lose all of
its history when newer reports from other identities fill the global pool. Set
`GlobalHistoryRetention` to `0` when every identity must retain its own history, or use stable
pipeline identities and separate history directories for independently bounded histories. A
positive global limit must be at least as large as `HistoryRetention`.
Report and history I/O failures are logged as warnings and do not replace a pipeline failure.
Report and history files are published atomically, so cancellation or a failed write cannot replace
a complete report with partial JSON. After each successful history save, the built-in store also
removes atomic-write temporary files older than 24 hours while leaving recent files for concurrent
writers.

Query retained reports newest-first through `IRunHistoryStore`:

```csharp
await foreach (var failedRun in historyStore.GetRunsAsync(new RunHistoryQuery
{
    PipelineIdentity = "release-pipeline",
    MaxRuns = 10,
    Since = DateTimeOffset.UtcNow.AddDays(-30),
    Status = Status.Failed,
}, cancellationToken))
{
    // Inspect failedRun.
}
```

`GetLatestAsync(pipelineIdentity, cancellationToken)` remains available as an extension method over
`GetRunsAsync`. The registered `IRunHistoryReader` provides measured, attributable module-duration
samples from the latest runs:

```csharp
var samples = await historyReader.GetModuleDurationTrendAsync(
    moduleTypeName,
    lastN: 10,
    cancellationToken);
```

Configure `RunReportOptions.PipelineIdentity` before using `IRunHistoryReader`; the reader uses that
identity to select the current pipeline's retained history. Schema-v1 reports remain queryable, but
they have no run ID and are therefore omitted from duration trends.

CI agents are often ephemeral, so restore the history directory from a cache before running the
pipeline. For example, a GitHub Actions workflow can restore the newest cache for its branch and
save the updated history under a run-specific key:

```yaml
- uses: actions/cache@v4
  with:
    path: .modularpipelines/run-history
    key: ${{ runner.os }}-modularpipelines-history-${{ github.ref_name }}-${{ github.run_id }}-${{ github.run_attempt }}
    restore-keys: |
      ${{ runner.os }}-modularpipelines-history-${{ github.ref_name }}-
```

## Custom history stores

Implement `IRunHistoryStore` to keep reports in a database, object store, or another backend, then
register it on the builder:

```csharp
builder.AddRunHistoryStore<MyRunHistoryStore>();
```

The store returns matching reports newest-first and saves the completed current report. Custom
stores own their retention behavior.

In v4, custom stores implement `GetRunsAsync(RunHistoryQuery, CancellationToken)`. The former
`GetLatestAsync` interface member is now an extension method, so stores need only implement the
query operation and `SaveAsync`.
