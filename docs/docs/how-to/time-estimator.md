---
title: Time Estimator
---

# Time Estimator

The console progress display and the scheduler's critical-path ordering use estimated module
durations. ModularPipelines registers a default provider, so estimates work without any setup:
it stores the latest measured duration of each module and sub-module as a small text file under
`%APPDATA%/ModularPipelines/EstimatedTimes` (the user's application-data folder on each platform).

Estimates are best-effort. If the provider cannot read or write its storage (for example a
read-only profile, or several pipelines saving at once), the failure is logged, a default
estimate of two minutes is used, and the module's outcome is unaffected. The default provider
replaces entries atomically, so concurrent pipelines never read a partially written value.

## Custom providers

Replace the default provider to share estimates between machines, or to derive them from
another source such as [run history](run-reports.md):

```csharp
var builder = Pipeline.CreateBuilder(args);

builder
    .AddModule<Module1>()
    .AddModule<Module2>();

builder.AddModuleEstimatedTimeProvider<MyEstimatedTimeProvider>();

await builder.RunAsync();
```

Implement `GetModuleEstimatedTimeAsync` and `SaveModuleTimeAsync`. The sub-module members have
default implementations (no stored estimates, and saving does nothing); override them when the
provider should also estimate sub-modules.

```csharp
public sealed class MyEstimatedTimeProvider(IMyDurationStore store) : IModuleEstimatedTimeProvider
{
    public async Task<TimeSpan> GetModuleEstimatedTimeAsync(
        ModuleId moduleId,
        CancellationToken cancellationToken = default)
    {
        return await store.GetAsync(moduleId.Value, cancellationToken)
            ?? TimeSpan.FromMinutes(2);
    }

    public Task SaveModuleTimeAsync(
        ModuleId moduleId,
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        return store.SaveAsync(moduleId.Value, duration, cancellationToken);
    }
}
```

When run history is enabled, `IRunHistoryReader.GetModuleDurationTrendAsync` returns recent
measured durations for a module, which a custom provider can average instead of keeping its own
storage. Configure `RunReportOptions.PipelineIdentity` explicitly through `builder.ConfigureOptions`
before querying the reader. The identity derived automatically when writing reports is not used by
`IRunHistoryReader`; a missing or blank identity causes `InvalidOperationException`. See
[Run reports and history](run-reports.md#local-history-and-deltas) for configuration and query examples.

All provider methods use `ModularPipelines.ModuleId`, shared with run history, artifacts, and
execution backends. Use `ModuleId.FromType(typeof(MyModule))` to derive an identifier. A
`[ModuleId("build")]` attribute keeps the identifier stable when a module is renamed. Construct
an identifier with `new ModuleId(value)` or an explicit cast from a string; invalid identifiers
throw during this explicit validation step. Reading an identifier as a string remains implicit.
The default provider hashes identifiers into safe filenames. Existing estimates are rebuilt
under the new keys after upgrading to v4.
