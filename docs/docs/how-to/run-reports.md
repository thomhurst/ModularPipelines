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

## Include module output excerpts

Module output is excluded by default. Opt in when reports need enough output to diagnose recent
failures without opening the full CI log:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    RunReport = options.RunReport with
    {
        IncludeModuleOutput = true,
        MaxOutputBytesPerModule = 8 * 1024,
    },
});
```

Each module gets one shared UTF-8 byte budget across its `stdoutTail` and `stderrTail`; the newest
output wins when the limit is reached. Console error and command-error output is retained in
`stderrTail`, while console output and other module logs are retained in `stdoutTail`. Excerpts are
taken only from secret-masked module buffers and are masked again when the report is created.

## Local history and deltas

By default, the `IRunHistoryStore` saves reports under `.modularpipelines/run-history` on local and
CI runs, even when JSON report writing is disabled. It retains the latest 20 reports and uses the
newest compatible report to calculate module and total-duration deltas. When a previous duration
exists, the final results table includes a `Δ previous` column.

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
        PipelineIdentity = "release-pipeline",
    },
});
```

History is partitioned by pipeline identity and pruning only removes files owned by the built-in
history store. When `PipelineIdentity` is omitted, Modular Pipelines derives one from the report
path and registered module types. History is bounded: after each save, the default store deletes
owned files beyond the configured limit for that pipeline.
Report and history I/O failures are logged as warnings and do not replace a pipeline failure.

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

The store returns the latest report for the requested pipeline identity and saves the completed
current report. Custom stores own their retention behavior.

In v4, `IRunHistoryStore` removes the parameterless `GetLatestAsync` member. Existing custom stores
must move any identity filtering into `GetLatestAsync(string pipelineIdentity, CancellationToken)`,
which is now the only read member to implement.
