---
title: Pipeline Execution Modes
---

# Pipeline Execution Modes

A pipeline has two execution modes:
- StopOnFirstException
- WaitForAllModules

By default, a pipeline will use `StopOnFirstException`. This means as soon as any module throws an exception, the pipeline is considered fail and will terminate. This is for fast feedback.

If you want to run every module regardless, you can switch to `WaitForAllModules`, and the pipeline won't terminate until all modules have finished executing.

## Example

```csharp
await PipelineHostBuilder.Create()
    .AddModule<Module1>()
    .AddModule<Module2>()
    .AddModule<Module3>()
    .ConfigurePipelineOptions((context, options) =>
    {
        options.ExecutionMode = ExecutionMode.WaitForAllModules;
    })
    .ExecutePipelineAsync();

```