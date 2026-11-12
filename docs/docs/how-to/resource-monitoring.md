---
title: Machine resource monitoring
---

# Machine resource monitoring

A pipeline that runs many modules in parallel can use all of the machine's CPU or memory. Every
module then slows down, and a slow module looks the same as an overloaded machine.

ModularPipelines samples machine-wide CPU and memory about once per second during a run. The values
cover the whole machine, including the child processes that modules start and anything else
running on it. On Linux with cgroup v2, memory can instead describe the container limit, and
available pressure counters detect stalls. Other platforms use system calls.

Monitoring is on by default on Linux, Windows, and macOS. On other platforms it does nothing.

## Warnings during the run

When CPU or memory stays at or above its threshold for several consecutive samples, the pipeline
logs a warning that names the modules running at that moment:

```text
Machine CPU usage has been at or above 90% since 14:02:11 (now 98%). Running modules: BuildModule, TestModule
```

On Linux, consecutive samples with new task stalls or CPU throttling also count as saturation,
even when utilization is below its threshold. These warnings explicitly name the stall signal:

```text
Machine CPU saturation since 14:02:11: Linux task stalls or CPU throttling detected (usage 12%). Running modules: BuildModule
```

A second message is logged at information level when saturation ends.

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
See [Run reports and history](./run-reports.md). CPU percentages remain host utilization, even
when container throttling triggers a period; they are not percentages of a container CPU quota.
A period's peak is measured utilization, so stall-triggered periods can have peaks below the
configured threshold.

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
| `SustainedSamples` | `3` | Consecutive samples above a usage threshold or with Linux stalls/throttling before a warning is logged. |

To turn monitoring off:

```csharp
builder.ConfigureResourceMonitoring(monitoring => monitoring with { Enabled = false });
```

## Linux containers and pressure

On Linux with cgroup v2, memory uses a finite `memory.max` limit and its matching `memory.current`
when the limit is at or below host memory. Each visible parent is checked against its own limit, and the cgroup with the highest
usage fraction supplies the reading. Memory includes other processes in that cgroup. Reclaimable
file cache (`inactive_file` from `memory.stat`) is subtracted from usage when available. Unlimited, invalid,
or unreadable cgroup data falls back to host memory from `/proc/meminfo`.

The monitor reads `cpu.stat` from the process cgroup and every visible ancestor, detecting increases in its `nr_throttled` or `throttled_usec` and in the `some`
stall-time total from `cpu.pressure` and `memory.pressure`. If cgroup pressure files are unavailable,
it reads `/proc/pressure/cpu` and `/proc/pressure/memory` instead; these cover the host. A PSI stall increase must cover at least 10% of the actual elapsed sample interval to count as a stall sample; brief routine contention is ignored. Any positive CPU throttling increase counts. Both signals remain subject to `SustainedSamples`. Each counter source has its own baseline, so losing one pressure file does not discard unrelated
counters. Historical totals alone do not trigger warnings. Missing counters are ignored without disabling ordinary monitoring.

See the kernel documentation for [cgroup v2](https://docs.kernel.org/admin-guide/cgroup-v2.html)
and [pressure stall information](https://docs.kernel.org/accounting/psi.html).

## Limits

- cgroup v1 limits are not read. CPU percentages always describe the host. Ancestor limits or throttling outside the visible cgroup mount cannot be observed. Ambiguous namespace-root bind mounts are used only when `cgroup.procs` confirms process membership; otherwise monitoring tries another mount or host data.
- In distributed runs, each process samples its own machine. Worker warnings and saturation periods name the modules executing on that worker. Worker figures are not combined into the master's report.
