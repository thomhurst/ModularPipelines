---
title: Runtime behavior changes
sidebar_position: 14
---

# Runtime behavior changes

Nothing on this page causes a compiler error. Read it once your pipeline compiles, and check the
items that apply to you.

Each entry says what changed, then what to do.

## Default run history

V4 saves run reports by default, on local and CI runs, even when explicit JSON report writing is
off. V3.2.8 did not.

- Reports go to `.modularpipelines/run-history`, relative to the Git repository root when there is
  one, or the pipeline working directory otherwise.
- Retained reports are the baseline for duration deltas.

**Keep it out of source control.** Add this to `.gitignore`:

```gitignore
.modularpipelines/run-history/
```

**Or turn it off.** Set `RunReport.HistoryRetention` to `0`:

```csharp
builder.ConfigureOptions(options => options with
{
    RunReport = options.RunReport with
    {
        HistoryRetention = 0,
    },
});
```

Turning it off also disables history-based duration deltas and successful-result reuse with
`--rerun-failed`. It does not disable explicit report output or automatic CI reports. See
[Run reports and history](../how-to/run-reports.md#local-history-and-deltas) to change the history
directory, retention limits, and report output settings.

## Modules and scheduling

- **Skipped required dependencies now cascade.** If a required dependency is skipped, its
  consumers are skipped too. Mark a dependency optional only if the consumer can really proceed
  without its result, and handle missing or skipped results explicitly.
- **Dependencies are validated even when a condition would skip the module.** Register required
  dependencies, or use the conditional or optional declaration you intend.
- **Fluent and attribute conditions share the execution pipeline, after dependency waiting.**
  Verify condition ordering, skip hooks, and inherited condition composition.
- **Module timeouts apply to each retry attempt**, not the whole retry sequence. Include attempts
  and backoff in your total runtime budget, and forward cancellation into the work.
- **`WithTimeout(TimeSpan.Zero)` is rejected.** It meant "no timeout" in V3. V4 rejects zero and
  negative values. Use `WithTimeout(Timeout.InfiniteTimeSpan)` to turn off a module timeout.
- **Always-run cleanup is scheduled after failures and cancellation**, with bounded progress
  waits. Test teardown under failure and cancellation. Keep cleanup bounded and idempotent.
- **Category matching and selection are validated more strictly.** Check your configured category
  and module selectors. Typos are no longer silently accepted.
- **Event handler failures follow fixed rules.** See
  [When a handler throws](./hooks-and-plugins.md#when-a-handler-throws). Set `ContinueOnError` on
  handlers that must not fail the module or pipeline.
- **Repeated registration helpers.** Helpers for single-instance services replace earlier
  registrations. Multi-instance helpers add each type once. Check that repeated `Add*` calls, such
  as a results repository or time estimator, register the implementation you expect.

## Commands and processes

- **Commands time out after 30 minutes by default.** In V3, `ExecutionTimeout` defaulted to `null`,
  and tool commands ran with no command-level limit. For a longer command, set a longer
  `ExecutionTimeout`, or `null` for no limit. The `TimeoutException` names the limit and this
  option. The module default is still 30 minutes, and is now configurable with
  `DefaultModuleTimeout`.
- **Captured output is capped** at 1,048,576 characters per stream by default. See
  [Output capture limit](#output-capture-limit).
- **Canceled commands terminate their descendant processes** after graceful cancellation. Remove
  any assumption that child processes survive a canceled pipeline.
- **Background processes left by a command that completed normally** have platform-specific
  lifetimes. On Windows, descendants in the command's job object are terminated when the pipeline
  is disposed. On Linux and macOS, they are left running. Stop background servers or daemons
  explicitly during cleanup on every platform.
- **PowerShell runs with `-NoProfile -NonInteractive` by default.** Scripts run through
  `context.Shell.PowerShell` that rely on profile-loaded modules, aliases, or prompts must set
  `NoProfile = false` or `NonInteractive = false` on the options.

### Output capture limit

The limit counts characters, not bytes. When a stream exceeds `MaxCapturedOutputLength`:

- The captured string keeps the beginning and the end, with
  `... [truncated N characters] ...` between them. The marker adds characters beyond the limit.
- `CommandResult.StandardOutputTruncatedCharacters` and
  `CommandResult.StandardErrorTruncatedCharacters` report how many characters were omitted from
  each stream. Zero means none. They are also available on `CommandException.Result`.
- At `CommandLogVerbosity.Normal` or higher, a warning names `MaxCapturedOutputLength` and reports
  both counts. `Silent` and `InputOnly` suppress the warning, so check the counts at those levels.

:::warning
Do not parse capped output as complete JSON, XML, or another machine-readable document. Raise the
limit, or set `MaxCapturedOutputLength = 0` (or a negative value) for unlimited capture when you
need it all and the memory cost is acceptable.
:::

## HTTP

- **Retries default to idempotent methods**, and need a replayable body of at most 1 MiB. V3
  retried all methods. Audit `POST`/`PATCH` and upload calls. Opt in with
  `HttpResilienceOptions.RetryEnabled` only when repeating the operation is safe. See
  [Retries](./logging-and-http.md#retries) for body limits and `AttemptTimeout`.
- **Request and response bodies are not logged by default.** Set `ShowRequestBody` /
  `ShowResponseBody`, or use `HttpLoggingOptions.Diagnostic`, where you need them.
- **Non-success responses throw by default.** See
  [Failure responses now throw](./logging-and-http.md#failure-responses-now-throw).

## Environment and configuration

- **`AddJsonFile` resolves relative paths against the pipeline working directory.** Check that
  `appsettings.json` is found when you run a published pipeline. See
  [Builder settings](./entry-point-and-options.md#builder-settings).
- **`IsCI`, `OnCI`, and `Require.Ci()` also honor a truthy `CI` variable.** `IsBuildServer` becomes
  `IsCI`; use `IsLocal` for the inverse. Check local runs that set `CI`. The startup log says when
  `CI` alone decided. See [Build system detection](./context.md#build-system-detection).
- **`Md5File` returns lowercase hex.** Compare hashes case-insensitively.
- Required result access, read-only options, generated command validation, and pipeline-scoped
  working directories also change behavior. They are described on their own pages. Include them in
  your migration acceptance tests.

## Telemetry and secrets

- **Secrets are masked in more output surfaces and transformed forms.** Test your redaction. Do
  not parse masked diagnostic command input or environment as raw execution data.
- **OpenTelemetry module status values are lowercase snake_case.** Update dashboards, alerts, and
  metric queries that filter `modular_pipelines.module.status`.

| V3 tag value | V4 tag value |
| --- | --- |
| `Successful` | `succeeded` |
| `Failed` | `failed` |
| `Failed` (for a timed-out module) | `timed_out` |
| `Skipped` | `skipped` |
| `IgnoredFailure` (on a separate `module.status` tag) | `failure_ignored` |

- Old case-sensitive filters such as `status == "Failed"` no longer match.
- A filter on `failed` no longer includes timeouts.
- Account for `failure_ignored` separately from `failed` when you update failure alerts.

The tag uses telemetry strings, not the `ModuleStatus` enum names. The full set of V4 values is
`not_started`, `running`, `succeeded`, `failed`, `failure_ignored`, `skipped`, `timed_out`,
`canceled`, `dependency_failed`, `restored_from_history`, `restored_from_cache`, and `unknown`.

See [OpenTelemetry](../how-to/opentelemetry.md) for the current instrumentation contract.
