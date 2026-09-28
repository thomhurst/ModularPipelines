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
        Type moduleType,
        CancellationToken cancellationToken = default)
    {
        return await store.GetAsync(moduleType.FullName!, cancellationToken)
            ?? TimeSpan.FromMinutes(2);
    }

    public Task SaveModuleTimeAsync(
        Type moduleType,
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        return store.SaveAsync(moduleType.FullName!, duration, cancellationToken);
    }
}
```

When run history is enabled, `IRunHistoryReader.GetModuleDurationTrendAsync` returns recent
measured durations for a module, which a custom provider can average instead of keeping its own
storage.
