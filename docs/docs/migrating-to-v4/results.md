---
title: Results and summaries
sidebar_position: 4
---

# Results and summaries

For the change to a module's own return type, see [Execute signatures](./modules.md#execute-signatures).

## Reading a result

`ModuleResult<T>` now has public typed `Failure` and `Skipped` variants alongside `Success`.
Replace `IsSuccess`, `IsFailure`, `IsSkipped`, and `ModuleResultType` checks with a pattern match or
an accessor.

```csharp
var result = await context.GetModule<BuildModule>();

// Pattern match.
if (result is ModuleResult<BuildOutput>.Success { Value: var output })
{
    // Use output.
}

// Throws for a failure or a skip. Can still return null.
var successfulOutput = result.Value;

// Reject null yourself when you need to.
var requiredOutput = result.Value
    ?? throw new InvalidOperationException("BuildModule returned no output.");

// Never throws. Keeps "no value" as default.
var optionalOutput = result.ValueOrDefault;
```

| Accessor | Failure or skip | Successful `null` |
| --- | --- | --- |
| `result.Value` | Throws | Returns `null` |
| `result.ValueOrDefault` | Returns the default | Returns `null` |

- **Do not turn every `.ValueOrDefault` into `.Value`.** That changes how failures and skips are
  handled.
- **`Value` does not check for null.** Non-nullable annotations are not enforced at runtime.
  Nullable-oblivious code, F#, or `null!` can still supply null, even for `Module<BuildOutput>`.
  Add an explicit check when null must be rejected.
- **Match the typed variants.** For a typed result, use `ModuleResult<T>.Failure` and
  `ModuleResult<T>.Skipped`, not the non-generic variants.
- **`ModuleResult<T>.Success` is no longer a positional record.** Positional patterns still work,
  but a named constructor argument is now `value:` rather than `Value:`.

## Result metadata

| V3 result member | V4 member |
| --- | --- |
| `ModuleName` | `Name` |
| `ModuleDuration` | `Duration` |
| `ModuleStart` / `ModuleEnd` | `StartTime` / `EndTime` |
| `ModuleStatus` | `Status` |

Across results, plans, timelines, and module run reports:

- `Name` is the display name.
- `ModuleId` is the stable identity, including IDs set with `[ModuleId]`.
- `TypeName` (results, plans, and timelines) is the type metadata.
- Hook contexts keep `ModuleName`.

`ModuleTimeline` follows the same renames: `ModuleName` / `ModuleTypeName` become `Name` /
`TypeName`, and `ExecutionType` becomes `ExecutionHint`.

Existing report and timeline JSON field names are preserved. Manually constructed or legacy results
can have a default `ModuleId` when their identity is unknown. The engine fills it in when it
restores a result.

## Statuses

The enum moved from `ModularPipelines.Enums.Status` to `ModularPipelines.ModuleStatus`.

| V3 enum value | V4 enum value |
| --- | --- |
| `NotYetStarted` | `NotStarted` |
| `Processing` | `Running` |
| `Successful` | `Succeeded` |
| `IgnoredFailure` | `FailureIgnored` |
| `UsedHistory` | `RestoredFromHistory` |
| `PipelineTerminated` | `Canceled` or `DependencyFailed`, depending on the cause |
| `Retried` | Removed; retries are attempts, not a terminal status |
| `Failed`, `Skipped`, `TimedOut`, `Unknown` | Same names on `ModuleStatus` |

- New statuses: `Canceled`, `DependencyFailed`, and `RestoredFromCache` (fingerprint-cache hits).
- V4 spells "canceled" with one L everywhere, as .NET's `OperationCanceledException` does.
- Status names also affect custom JSON consumers, stored results, dashboards, and exhaustive
  switches. Do not rely on the old numeric enum values.
- Built-in run history and stored results still read the old names. `PipelineTerminated` and
  `Cancelled` load as `Canceled`.

## Pipeline summaries

The summary exposes results rather than live modules.

| V3 | V4 |
| --- | --- |
| `await summary.GetModuleResultsAsync()` | `summary.Results` |
| `summary.GetFailedModuleResults()` | `summary.Failures` plus `summary.IgnoredFailures` |
| `summary.Status` | `summary.Succeeded` |
| `summary.Start` / `End` / `TotalDuration` | `StartTime` / `EndTime` / `Duration` |
| `summary.Modules`, `summary.GetModule<T>()` | Removed; use `summary.GetResult<TModule, TResult>()` |

`PipelineSummary` is now sealed. Update custom JSON consumers for the renamed timing properties.

### Failures

V3's `GetFailedModuleResults()` mixed two kinds of result. V4 splits them:

- `summary.Failures` holds the failures that fail the pipeline.
- `summary.IgnoredFailures` holds results with status `FailureIgnored`. They still have an
  exception, but they do not fail the pipeline.

To get the V3 list, combine both.

### Overall outcome

Read `summary.Succeeded`. It is `true` only when the pipeline completes without unignored failures.

| Allows success | Prevents success |
| --- | --- |
| Skipped, cached, restored, and ignored-failure results | Incomplete, failed, and canceled runs |

The value survives JSON round-trips, even though module results are not serialized.

### Looking up one module's result

Inside a module, keep using `context.GetModule<T>()`. After the run, use the summary:

```csharp
var result = summary.GetResult<BuildModule, CommandResult>();

if (summary.TryGetResult<BuildModule, CommandResult>(out var completed)
    && completed.TryGetValue(out var commandResult))
{
    // Use the successful commandResult.
}
```

- `GetResult` throws `InvalidOperationException` when there is no completed result. `TryGetResult`
  returns `false`.
- Finding a result does not mean it succeeded. Both methods also return failure and skipped
  variants. Check the variant or use `TryGetValue` before you read `.Value`.
- Lookup uses the exact module type, so two modules that return the same value type stay distinct.
- Use `summary.Results` to enumerate.
- Results are excluded from JSON serialization, so a deserialized summary has none to look up.
  Update custom serialization and result repositories for the new metadata.

## If you used a V4 pre-release build

`PipelinePlanModule` and `ModuleRunReport` are new in V4. Since the pre-release builds:

- `PipelinePlanModule.Name` is now the short type name. `TypeName` is the fully qualified label.
- `ModuleRunReport.ModuleName` / `Start` / `End` are now `Name` / `StartTime` / `EndTime`.
- On a report, `ResultStartTime` / `ResultEndTime` still describe the original stored result when
  it is reused. `StartTime` / `EndTime` describe execution in the current run.
