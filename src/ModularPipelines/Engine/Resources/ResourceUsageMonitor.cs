using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.Tracing;

namespace ModularPipelines.Engine.Resources;

internal sealed class ResourceUsageMonitor(
    IOptions<PipelineOptions> options,
    IMetricsCollector metricsCollector,
    ILogger<ResourceUsageMonitor> logger,
    TimeProvider timeProvider,
    IMachineResourceReader? reader = null) : IResourceUsageMonitor
{
    // Usage that hovers around a threshold must not flood the log or the run report.
    internal const int MaxPeriods = 50;

    private readonly ResourceMonitoringOptions _options = options.Value.ResourceMonitoring;
    private readonly Lock _lock = new();
    private readonly List<ResourceSaturationPeriod> _periods = [];
    private readonly SaturationTracker _cpu = new(ResourceKind.Cpu, "CPU");
    private readonly SaturationTracker _memory = new(ResourceKind.Memory, "memory");

    private MachineResourceReading? _previous;
    private bool _monitoring;
    private int _sampleCount;
    private double _cpuSum;
    private double _peakCpu;
    private double _peakMemory;

    public IAsyncDisposable Start()
    {
        if (reader is null || !_options.Enabled || _options.SampleInterval <= TimeSpan.Zero)
        {
            return NoopScope.Instance;
        }

        lock (_lock)
        {
            _monitoring = true;
        }

        return new Scope(this);
    }

    public ResourceUsage? GetResult()
    {
        lock (_lock)
        {
            if (_sampleCount == 0)
            {
                return null;
            }

            return new ResourceUsage
            {
                PeakCpu = _peakCpu,
                AverageCpu = _cpuSum / _sampleCount,
                PeakMemory = _peakMemory,
                SampleCount = _sampleCount,
                SaturatedDuration = GetSaturatedDuration(_periods),
                Periods = [.. _periods.OrderBy(static period => period.Start)],
            };
        }
    }

    internal static TimeSpan GetSaturatedDuration(IEnumerable<ResourceSaturationPeriod> periods)
    {
        var total = TimeSpan.Zero;
        DateTimeOffset? coveredUntil = null;
        foreach (var period in periods.OrderBy(static period => period.Start))
        {
            var start = coveredUntil is { } covered && covered > period.Start ? covered : period.Start;
            if (period.End > start)
            {
                total += period.End - start;
                coveredUntil = period.End;
            }
        }

        return total;
    }

    /// <summary>
    /// Takes one sample. Returns <see langword="false"/> when the machine counters cannot be read.
    /// </summary>
    internal bool Sample()
    {
        if (reader is null || !reader.TryRead(out var reading))
        {
            return false;
        }

        var previous = _previous;
        _previous = reading;
        if (previous is not { } last)
        {
            // CPU usage is the change between two readings.
            return true;
        }

        var totalDelta = reading.CpuTotal - last.CpuTotal;
        var idleDelta = reading.CpuIdle - last.CpuIdle;
        if (totalDelta <= 0 || idleDelta < 0 || reading.MemoryTotalBytes <= 0)
        {
            // Counters did not advance, or wrapped.
            return true;
        }

        var cpu = Math.Clamp(1 - ((double) idleDelta / totalDelta), 0, 1);
        var memory = Math.Clamp(1 - ((double) reading.MemoryAvailableBytes / reading.MemoryTotalBytes), 0, 1);
        var now = timeProvider.GetUtcNow();

        lock (_lock)
        {
            if (_monitoring)
            {
                ModuleActivityTracing.RecordMachineUsage(this, cpu, memory);
            }

            _sampleCount++;
            _cpuSum += cpu;
            _peakCpu = Math.Max(_peakCpu, cpu);
            _peakMemory = Math.Max(_peakMemory, memory);
            Track(_cpu, cpu, _options.CpuThreshold, now);
            Track(_memory, memory, _options.MemoryThreshold, now);
        }

        return true;
    }

    private int OpenPeriodCount => (_cpu.IsOpen ? 1 : 0) + (_memory.IsOpen ? 1 : 0);

    private static int ToPercent(double fraction) => (int) Math.Round(fraction * 100);

    private void Track(SaturationTracker tracker, double value, double threshold, DateTimeOffset now)
    {
        if (value < threshold)
        {
            Close(tracker, now);
            return;
        }

        if (tracker.Consecutive == 0)
        {
            tracker.Start = now;
            tracker.Peak = 0;
            tracker.Modules.Clear();
        }

        tracker.Consecutive++;
        tracker.Peak = Math.Max(tracker.Peak, value);
        foreach (var module in metricsCollector.GetRunningModules())
        {
            tracker.Modules.Add(module.Name);
        }

        if (tracker.IsOpen
            || tracker.Consecutive < Math.Max(1, _options.SustainedSamples)
            || _periods.Count + OpenPeriodCount >= MaxPeriods)
        {
            return;
        }

        tracker.IsOpen = true;
        logger.LogWarning(
            "Machine {Resource} usage has been at or above {Threshold}% since {Start:HH:mm:ss} (now {Usage}%). Running modules: {Modules}",
            tracker.DisplayName,
            ToPercent(threshold),
            tracker.Start.ToLocalTime(),
            ToPercent(value),
            tracker.Modules.Count == 0 ? "none" : string.Join(", ", tracker.Modules));
    }

    private void Close(SaturationTracker tracker, DateTimeOffset now)
    {
        if (tracker.IsOpen)
        {
            _periods.Add(new ResourceSaturationPeriod
            {
                Kind = tracker.Kind,
                Start = tracker.Start,
                End = now,
                Peak = tracker.Peak,
                Modules = [.. tracker.Modules],
            });
            logger.LogInformation(
                "Machine {Resource} usage dropped below its threshold after {Seconds:F0}s (peak {Peak}%)",
                tracker.DisplayName,
                (now - tracker.Start).TotalSeconds,
                ToPercent(tracker.Peak));
        }

        tracker.IsOpen = false;
        tracker.Consecutive = 0;
    }

    private void Stop()
    {
        try
        {
            // A final sample covers runs shorter than the sample interval.
            Sample();
        }
        catch (Exception exception)
        {
            logger.LogDebug(exception, "Final resource sample failed");
        }

        var now = timeProvider.GetUtcNow();
        lock (_lock)
        {
            _monitoring = false;
            ModuleActivityTracing.RemoveMachineUsage(this);
            Close(_cpu, now);
            Close(_memory, now);
        }
    }

    private async Task SamplePeriodicallyAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var timer = new PeriodicTimer(_options.SampleInterval, timeProvider);
            do
            {
                if (!Sample())
                {
                    logger.LogDebug("Machine resource counters are unavailable; resource monitoring is off for this run");
                    return;
                }
            }
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            // Normal scope teardown.
        }
        catch (Exception exception)
        {
            // Monitoring is diagnostic only and must never fail the run.
            logger.LogDebug(exception, "Resource monitoring stopped after an error");
        }
        finally
        {
            lock (_lock)
            {
                _monitoring = false;
                ModuleActivityTracing.RemoveMachineUsage(this);
            }
        }
    }

    private sealed class SaturationTracker(ResourceKind kind, string displayName)
    {
        public ResourceKind Kind { get; } = kind;

        public string DisplayName { get; } = displayName;

        public SortedSet<string> Modules { get; } = new(StringComparer.Ordinal);

        public int Consecutive { get; set; }

        public bool IsOpen { get; set; }

        public DateTimeOffset Start { get; set; }

        public double Peak { get; set; }
    }

    private sealed class NoopScope : IAsyncDisposable
    {
        public static readonly NoopScope Instance = new();

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class Scope : IAsyncDisposable
    {
        private readonly ResourceUsageMonitor _monitor;
        private readonly CancellationTokenSource _cancellation = new();
        private readonly Task _task;
        private int _disposed;

        public Scope(ResourceUsageMonitor monitor)
        {
            _monitor = monitor;
            _task = monitor.SamplePeriodicallyAsync(_cancellation.Token);
        }

        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }

            await _cancellation.CancelAsync().ConfigureAwait(false);
            await _task.ConfigureAwait(false);
            _cancellation.Dispose();
            _monitor.Stop();
        }
    }
}
