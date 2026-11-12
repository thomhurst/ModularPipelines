using System.Diagnostics.Metrics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Resources;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;
using ModularPipelines.UnitTests.Logging;
using Moq;

namespace ModularPipelines.UnitTests.Engine.Resources;

// These tests observe process-wide gauges and must exclude other pipeline tests.
[TUnit.Core.NotInParallel]
public class ResourceUsageMonitorTests
{
    private static readonly DateTimeOffset Start = new(2026, 1, 1, 12, 0, 0, TimeSpan.Zero);

    [Test]
    public async Task Gauges_ReportLatestActiveSample_AndStopAfterDisposal()
    {
        var measurements = new Dictionary<string, double>();
        var instruments = new List<Instrument>();
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == PipelineTelemetry.MeterName
                && (instrument.Name == PipelineTelemetry.MachineCpuUtilizationMetric
                    || instrument.Name == PipelineTelemetry.MachineMemoryUtilizationMetric))
            {
                instruments.Add(instrument);
                meterListener.EnableMeasurementEvents(instrument);
            }
        };
        listener.SetMeasurementEventCallback<double>((instrument, value, tags, state) => measurements[instrument.Name] = value);
        listener.Start();

        // Initialize the meter without publishing a sample.
        _ = ModuleActivityTracing.TelemetryMeter;
        listener.RecordObservableInstruments();
        await Assert.That(measurements).IsEmpty();
        await Assert.That(instruments.Count).IsEqualTo(2);
        await Assert.That(instruments.All(instrument => instrument is ObservableGauge<double> && instrument.Unit == "1")).IsTrue();

        var first = new Harness();
        await using var firstScope = first.Monitor.Start();
        first.Sample(0.25, 0.75);
        var readCount = first.Reader.ReadCount;
        listener.RecordObservableInstruments();
        await Assert.That(first.Reader.ReadCount).IsEqualTo(readCount);
        await Assert.That(measurements[PipelineTelemetry.MachineCpuUtilizationMetric]).IsEqualTo(0.25);
        await Assert.That(measurements[PipelineTelemetry.MachineMemoryUtilizationMetric]).IsEqualTo(0.75);

        var second = new Harness();
        await using var secondScope = second.Monitor.Start();
        second.Sample(0.5, 0.6);
        listener.RecordObservableInstruments();
        await Assert.That(measurements[PipelineTelemetry.MachineCpuUtilizationMetric]).IsEqualTo(0.5);
        await Assert.That(measurements[PipelineTelemetry.MachineMemoryUtilizationMetric]).IsEqualTo(0.6);

        await secondScope.DisposeAsync();
        listener.RecordObservableInstruments();
        await Assert.That(measurements[PipelineTelemetry.MachineCpuUtilizationMetric]).IsEqualTo(0.25);

        await firstScope.DisposeAsync();
        measurements.Clear();
        listener.RecordObservableInstruments();
        await Assert.That(measurements).IsEmpty();

        var disabled = new Harness(options => options with { Enabled = false });
        await using var disabledScope = disabled.Monitor.Start();
        listener.RecordObservableInstruments();
        await Assert.That(measurements).IsEmpty();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(10_000)]
    public async Task Gauges_StopWhenPeriodicSamplingFails(bool throws, CancellationToken cancellationToken)
    {
        var measurements = new Dictionary<string, double>();
        using var listener = new MeterListener();
        listener.InstrumentPublished = (instrument, meterListener) =>
        {
            if (instrument.Meter.Name == PipelineTelemetry.MeterName
                && (instrument.Name == PipelineTelemetry.MachineCpuUtilizationMetric
                    || instrument.Name == PipelineTelemetry.MachineMemoryUtilizationMetric))
            {
                meterListener.EnableMeasurementEvents(instrument);
            }
        };
        listener.SetMeasurementEventCallback<double>((instrument, value, tags, state) => measurements[instrument.Name] = value);
        listener.Start();

        var harness = new Harness();
        await using var scope = harness.Monitor.Start();
        listener.RecordObservableInstruments();
        await Assert.That(measurements.Count).IsEqualTo(2);

        harness.Reader.Throw = throws;
        harness.Reader.Available = false;
        harness.Time.Advance(TimeSpan.FromSeconds(1));

        // The timer continuation may run asynchronously; observe cleanup before disposing the scope.
        do
        {
            await Task.Delay(10, cancellationToken);
            measurements.Clear();
            listener.RecordObservableInstruments();
        }
        while (measurements.Count != 0);

        // A later successful read must not publish after periodic monitoring has stopped.
        harness.Reader.Throw = false;
        harness.Reader.Available = true;
        harness.Monitor.Sample();
        listener.RecordObservableInstruments();
        await Assert.That(measurements).IsEmpty();
        await Assert.That(harness.Monitor.GetResult()).IsNotNull();
    }

    [Test]
    public async Task Sample_RecordsPeriodOnlyAfterSustainedSamples_AndNamesRunningModules()
    {
        var harness = new Harness();
        harness.RunningModules = [typeof(BusyModule)];

        // Two samples over the threshold are not yet sustained.
        harness.Sample(cpu: 0.95);
        harness.Sample(cpu: 0.99);
        await Assert.That(harness.Log.ToString()).DoesNotContain("CPU usage");

        harness.RunningModules = [typeof(BusyModule), typeof(OtherModule)];
        harness.Sample(cpu: 0.97);
        await Assert.That(harness.Log.ToString())
            .Contains("Machine CPU usage has been at or above 90%")
            .And.Contains("Running modules: BusyModule, OtherModule");

        harness.Sample(cpu: 0.20);

        var usage = harness.Monitor.GetResult()!;
        var period = usage.Periods.Single();
        using (Assert.Multiple())
        {
            await Assert.That(usage.SampleCount).IsEqualTo(4);
            await Assert.That(usage.PeakCpu).IsEqualTo(0.99).Within(0.001);
            await Assert.That(usage.AverageCpu).IsEqualTo((0.95 + 0.99 + 0.97 + 0.20) / 4).Within(0.001);
            await Assert.That(usage.PeakMemory).IsEqualTo(0.5).Within(0.001);
            await Assert.That(period.Kind).IsEqualTo(ResourceKind.Cpu);
            await Assert.That(period.Start).IsEqualTo(Start.AddSeconds(1));
            await Assert.That(period.End).IsEqualTo(Start.AddSeconds(4));
            await Assert.That(period.Peak).IsEqualTo(0.99).Within(0.001);
            await Assert.That(period.Modules).IsEquivalentTo(["BusyModule", "OtherModule"]);
            await Assert.That(usage.SaturatedDuration).IsEqualTo(TimeSpan.FromSeconds(3));
        }
    }

    [Test]
    public async Task Sample_ShortSpike_IsNotRecorded()
    {
        var harness = new Harness();

        harness.Sample(cpu: 0.99);
        harness.Sample(cpu: 0.99);
        harness.Sample(cpu: 0.10);
        harness.Sample(cpu: 0.99);

        await Assert.That(harness.Monitor.GetResult()!.Periods).IsEmpty();
        await Assert.That(harness.Log.ToString()).IsEmpty();
    }

    [Test]
    public async Task Sample_TracksMemoryIndependently()
    {
        var harness = new Harness();

        for (var index = 0; index < 3; index++)
        {
            harness.Sample(cpu: 0.10, memory: 0.96);
        }

        await Assert.That(harness.Log.ToString()).Contains("Machine memory usage has been at or above 90%");
        await Assert.That(harness.Monitor.GetResult()!.PeakMemory).IsEqualTo(0.96).Within(0.001);
    }

    [Test]
    public async Task Sample_NeverRecordsMoreThanMaxPeriods()
    {
        var harness = new Harness(options => options with { SustainedSamples = 1 });
        for (var index = 0; index < ResourceUsageMonitor.MaxPeriods - 1; index++)
        {
            harness.Sample(cpu: 0.99);
            harness.Sample(cpu: 0.10);
        }

        // One slot is left; CPU and memory saturate in the same sample.
        harness.Sample(cpu: 0.99, memory: 0.99);
        harness.Sample(cpu: 0.10);
        harness.Sample(cpu: 0.99);
        harness.Sample(cpu: 0.10);

        await Assert.That(harness.Monitor.GetResult()!.Periods.Count).IsEqualTo(ResourceUsageMonitor.MaxPeriods);
    }

    [Test]
    [Timeout(30_000)]
    public async Task Scope_SamplesOnEachTick_AndClosesOpenPeriodOnDispose(CancellationToken cancellationToken)
    {
        var harness = new Harness(options => options with { SustainedSamples = 1 });
        harness.Reader.Cpu = 0.95;

        var scope = harness.Monitor.Start();
        for (var tick = 1; tick <= 3; tick++)
        {
            var read = harness.Reader.WaitForNextReadAsync();
            harness.Time.Advance(TimeSpan.FromSeconds(1));
            await read.WaitAsync(cancellationToken);
        }

        await scope.DisposeAsync();
        await scope.DisposeAsync();

        var usage = harness.Monitor.GetResult()!;
        await Assert.That(usage.SampleCount).IsGreaterThanOrEqualTo(3);
        await Assert.That(usage.Periods.Single().End).IsEqualTo(Start.AddSeconds(3));
    }

    [Test]
    public async Task Start_WhenDisabledOrUnsupported_DoesNothing()
    {
        var disabled = new Harness(options => options with { Enabled = false });
        var unsupported = new Harness(hasReader: false);

        await disabled.Monitor.Start().DisposeAsync();
        await unsupported.Monitor.Start().DisposeAsync();

        // Only the harness baseline read happened.
        await Assert.That(disabled.Reader.ReadCount).IsEqualTo(1);
        await Assert.That(disabled.Monitor.GetResult()).IsNull();
        await Assert.That(unsupported.Monitor.GetResult()).IsNull();
    }

    [Test]
    public async Task Start_WhenReaderThrows_DoesNotThrow()
    {
        var harness = new Harness();
        harness.Reader.Throw = true;

        await harness.Monitor.Start().DisposeAsync();

        await Assert.That(harness.Monitor.GetResult()).IsNull();
    }

    [Test]
    public async Task GetSaturatedDuration_CountsOverlappingPeriodsOnce()
    {
        var duration = ResourceUsageMonitor.GetSaturatedDuration(
        [
            new ResourceSaturationPeriod { Start = Start.AddSeconds(20), End = Start.AddSeconds(25) },
            new ResourceSaturationPeriod { Start = Start, End = Start.AddSeconds(10) },
            new ResourceSaturationPeriod { Start = Start.AddSeconds(5), End = Start.AddSeconds(12) },
            new ResourceSaturationPeriod { Start = Start.AddSeconds(6), End = Start.AddSeconds(8) },
        ]);

        await Assert.That(duration).IsEqualTo(TimeSpan.FromSeconds(17));
    }

    [Test]
    public async Task LinuxReader_ParsesCpuLine()
    {
        var parsed = LinuxMachineResourceReader.TryParseCpu(
            "cpu  100 20 30 400 50 6 7 8 9 10", out var idle, out var total);

        await Assert.That(parsed).IsTrue();
        await Assert.That(idle).IsEqualTo(450);
        await Assert.That(total).IsEqualTo(621);
    }

    [Test]
    [Arguments(null)]
    [Arguments("")]
    [Arguments("cpu0 1 2 3 4 5")]
    [Arguments("cpu 1 2 x 4 5")]
    public async Task LinuxReader_RejectsUnexpectedCpuLine(string? line)
    {
        await Assert.That(LinuxMachineResourceReader.TryParseCpu(line, out _, out _)).IsFalse();
    }

    [Test]
    public async Task LinuxReader_ParsesMemory()
    {
        var parsed = LinuxMachineResourceReader.TryParseMemory(
            ["MemTotal:       16000000 kB", "MemFree:          100000 kB", "MemAvailable:    4000000 kB", "Buffers: 1 kB"],
            out var total,
            out var available);

        await Assert.That(parsed).IsTrue();
        await Assert.That(total).IsEqualTo(16_000_000L * 1024);
        await Assert.That(available).IsEqualTo(4_000_000L * 1024);
    }

    [Test]
    public async Task LinuxReader_RejectsMemoryWithoutAvailable()
    {
        await Assert.That(LinuxMachineResourceReader.TryParseMemory(["MemTotal: 1 kB"], out _, out _)).IsFalse();
    }

    [Test]
    public async Task PlatformReader_ReturnsPlausibleValues()
    {
        IMachineResourceReader? reader = OperatingSystem.IsLinux() ? new LinuxMachineResourceReader()
            : OperatingSystem.IsWindows() ? new WindowsMachineResourceReader()
            : OperatingSystem.IsMacOS() ? new MacMachineResourceReader()
            : null;
        if (reader is null)
        {
            return;
        }

        await Assert.That(reader.TryRead(out var first)).IsTrue();
        await Task.Delay(200);
        await Assert.That(reader.TryRead(out var second)).IsTrue();

        using (Assert.Multiple())
        {
            await Assert.That(second.MemoryTotalBytes).IsGreaterThan(0);
            await Assert.That(second.MemoryAvailableBytes).IsBetween(0, second.MemoryTotalBytes);
            // macOS serves cached counters to frequent callers, so the total may not advance here.
            await Assert.That(second.CpuTotal).IsGreaterThanOrEqualTo(first.CpuTotal);
            await Assert.That(second.CpuIdle).IsGreaterThanOrEqualTo(first.CpuIdle);
            await Assert.That(second.CpuIdle - first.CpuIdle).IsLessThanOrEqualTo(second.CpuTotal - first.CpuTotal);
        }
    }

    [Test]
    public async Task MetricsCollector_GetRunningModules_ReturnsStartedAndNotCompleted()
    {
        var collector = new MetricsCollector();
        collector.RecordModuleQueued(typeof(OtherModule), Start);
        collector.RecordModuleStarted(typeof(BusyModule), Start);
        await Assert.That(collector.GetRunningModules()).IsEquivalentTo([typeof(BusyModule)]);

        collector.RecordModuleCompleted(typeof(BusyModule), Start, success: true, skipped: false, ModuleStatus.Succeeded);
        await Assert.That(collector.GetRunningModules()).IsEmpty();
    }

    [Test]
    public async Task Pipeline_ReportsResourceUsage()
    {
        // A fake reader keeps this deterministic: real counters may not advance during a short run.
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<IMachineResourceReader>(new FakeReader { Cpu = 0.5 });

        var summary = await builder
            .AddModule<BusyModule>()
            .RunAsync();

        await Assert.That(summary.ResourceUsage).IsNotNull();
        await Assert.That(summary.ResourceUsage!.PeakCpu).IsEqualTo(0.5).Within(0.001);
    }

    [Test]
    public async Task Pipeline_OmitsResourceUsage_WhenDisabled()
    {
        var summary = await TestPipelineBuilder.Create()
            .ConfigureResourceMonitoring(monitoring => monitoring with { Enabled = false })
            .AddModule<BusyModule>()
            .RunAsync();

        await Assert.That(summary.ResourceUsage).IsNull();
    }

    public class BusyModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(100, cancellationToken);
            return "Done";
        }
    }

    public class OtherModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("Done");
    }

    private sealed class Harness
    {
        public Harness(Func<ResourceMonitoringOptions, ResourceMonitoringOptions>? configure = null, bool hasReader = true)
        {
            var metrics = new Mock<IMetricsCollector>();
            metrics.Setup(x => x.GetRunningModules()).Returns(() => RunningModules);
            Monitor = new ResourceUsageMonitor(
                Microsoft.Extensions.Options.Options.Create(new PipelineOptions
                {
                    ResourceMonitoring = (configure ?? (static options => options))(new ResourceMonitoringOptions()),
                }),
                metrics.Object,
                new StringLogger<ResourceUsageMonitor>(Log, Microsoft.Extensions.Logging.LogLevel.Information),
                Time,
                hasReader ? Reader : null);

            // CPU usage needs a baseline reading.
            Monitor.Sample();
        }

        public FakeTimeProvider Time { get; } = new(Start);

        public FakeReader Reader { get; } = new();

        public StringBuilder Log { get; } = new();

        public IReadOnlyList<Type> RunningModules { get; set; } = [];

        public ResourceUsageMonitor Monitor { get; }

        public void Sample(double cpu, double memory = 0.5)
        {
            Reader.Cpu = cpu;
            Reader.Memory = memory;
            Time.Advance(TimeSpan.FromSeconds(1));
            Monitor.Sample();
        }
    }

    private sealed class FakeReader : IMachineResourceReader
    {
        private long _idle;
        private long _total;
        private TaskCompletionSource? _nextRead;

        public double Cpu { get; set; }

        public double Memory { get; set; } = 0.5;

        public bool Throw { get; set; }

        public bool Available { get; set; } = true;

        public int ReadCount { get; private set; }

        public Task WaitForNextReadAsync()
        {
            _nextRead = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
            return _nextRead.Task;
        }

        public bool TryRead(out MachineResourceReading reading)
        {
            if (Throw)
            {
                throw new IOException("counters unavailable");
            }

            if (!Available)
            {
                reading = default;
                return false;
            }

            ReadCount++;
            _total += 1000;
            _idle += (long) Math.Round(1000 * (1 - Cpu));
            reading = new MachineResourceReading(_idle, _total, 1000, (long) Math.Round(1000 * (1 - Memory)));
            _nextRead?.TrySetResult();
            return true;
        }
    }
}
