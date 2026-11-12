---
title: Machine resource monitoring
---

# Machine resource monitoring

A pipeline that runs many modules in parallel can use all of the machine's CPU or memory. Every
module then slows down, and a slow module looks the same as an overloaded machine.

ModularPipelines samples machine-wide CPU and memory about once per second during a run. The values
cover the whole machine, including the child processes that modules start and anything else
running on it. Sampling costs one small read or system call per interval.

Monitoring is on by default on Linux, Windows, and macOS. On other platforms it does nothing.

## Warnings during the run

When CPU or memory stays at or above its threshold for several consecutive samples, the pipeline
logs a warning that names the modules running at that moment:

```text
Machine CPU usage has been at or above 90% since 14:02:11 (now 98%). Running modules: BuildModule, TestModule
```

A second message is logged at information level when usage drops back below the threshold.

The listed modules were running during the period. The list does not say which of them caused the load.

## Results output

The final results output includes one line for the run:

```text
Machine: CPU peak 98%, avg 71%; memory peak 83%; saturated for 2m 10s
```

`saturated for` appears only when at least one warning was logged.

## Run report

`PipelineSummary.ResourceUsage` and `PipelineRunReport.ResourceUsage` contain the same figures as
fractions from 0 to 1, plus each saturation period with its start, end, peak, and module names.
See [Run reports and history](./run-reports.md).

A run records at most 50 saturation periods. The value is `null` when monitoring is disabled or
unsupported, or when no sample could be taken.

## Acting on it

If the same modules appear in saturation warnings, restrict how many of them run at once with
`[NotInParallel]`, a parallel limiter, or a lower `Concurrency.MaxParallelism`.
See [Parallelization](./parallelization.md).

## Configuration

```csharp
builder.ConfigureResourceMonitoring(monitoring => monitoring with
{
    SampleInterval = TimeSpan.FromSeconds(2),
    CpuThreshold = 0.95,
    MemoryThreshold = 0.85,
    SustainedSamples = 5,
});
```

| Option | Default | Meaning |
| --- | --- | --- |
| `Enabled` | `true` | Whether the machine is sampled. |
| `SampleInterval` | 1 second | Time between samples. |
| `CpuThreshold` | `0.9` | Fraction of CPU in use that counts as saturated. |
| `MemoryThreshold` | `0.9` | Fraction of memory in use that counts as saturated. |
| `SustainedSamples` | `3` | Consecutive samples at or above a threshold before a warning is logged. |

To turn monitoring off:

```csharp
builder.ConfigureResourceMonitoring(monitoring => monitoring with { Enabled = false });
```

## Limits

- Inside a container, the figures describe the host, not the container's CPU or memory limit.
- In distributed runs, each process samples its own machine. Worker warnings and saturation periods name the modules executing on that worker. Worker figures are not combined into the master's report.
