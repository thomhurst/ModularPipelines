---
title: Console Progress
---

# Console Progress

If you are using an interactive terminal, then a progress dialog will be displayed to you. This will attempt to show you estimated remaining time, and the current progress of all executing modules.

![image](https://github.com/thomhurst/ModularPipelines/assets/30480171/7d85af1e-abfd-40c4-8ef6-5df06baa88d6)

## Long-running module output

Buffered output from modules that are still running is flushed once per minute by default.
This preserves recent diagnostic output if the process is killed before normal pipeline
teardown. Incremental sections use an ellipsis (`…`) because the module has not completed.

Configure the interval globally. Setting it to zero disables time-based flushing, but the
entry threshold still protects against unbounded buffering:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    Console = options.Console with
    {
        ModuleOutputFlushInterval = TimeSpan.FromSeconds(30),
    },
});
```

To keep all output buffered until each module completes, disable both triggers:

```csharp
builder.ConfigurePipelineOptions(options => options with
{
    Console = options.Console with
    {
        ModuleOutputFlushInterval = TimeSpan.Zero,
        ModuleOutputFlushThreshold = 0,
    },
});
```

`Console.ModuleOutputFlushThreshold` counts entries, not bytes. Its default is 1,000 entries per
module.
