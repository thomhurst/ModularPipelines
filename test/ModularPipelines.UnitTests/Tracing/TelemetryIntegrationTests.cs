using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;

namespace ModularPipelines.UnitTests.Tracing;

[NotInParallel]
public class TelemetryIntegrationTests
{
    private const string Secret = "telemetry-secret-value";

    private sealed class SuccessfulCommandInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult<CommandResult?>(CommandResult.Ok());
        }
    }

    private sealed class CommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            context.Services.Get<ISecretRegistry>().AddSecret(Secret);
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("telemetry-tool")
                {
                    Arguments = [Secret],
                },
                cancellationToken: cancellationToken);
        }
    }

    private sealed class RetriedModule : Module<bool>
    {
        private int _attempts;

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithRetry(2, TimeSpan.Zero)
            .Build();

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Interlocked.Increment(ref _attempts) < 3
                ? Task.FromException<bool>(new InvalidOperationException("Retry me"))
                : Task.FromResult(true);
        }
    }

    [Test]
    public async Task Pipeline_Module_And_Command_Activities_Are_Parented_And_Obfuscated()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineHostBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await builder.AddModule<CommandModule>().ExecutePipelineAsync();

        var pipelineActivity = stoppedActivities.Single(activity => activity.OperationName == "Pipeline.Run");
        var moduleActivity = stoppedActivities.Single(activity => activity.OperationName == $"Module.{nameof(CommandModule)}");
        var commandActivity = stoppedActivities.Single(activity => activity.OperationName == "Command.telemetry-tool");
        var commandInput = commandActivity.GetTagItem(ModuleActivityTracing.CommandInputTag)?.ToString();

        using (Assert.Multiple())
        {
            await Assert.That(moduleActivity.ParentSpanId).IsEqualTo(pipelineActivity.SpanId);
            await Assert.That(commandActivity.ParentSpanId).IsEqualTo(moduleActivity.SpanId);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandToolTag))
                .IsEqualTo("telemetry-tool");
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandExitCodeTag))
                .IsEqualTo(0);
            await Assert.That(commandInput).Contains("**********");
            await Assert.That(commandInput).DoesNotContain(Secret);
        }
    }

    [Test]
    public async Task Module_Metrics_Record_Duration_And_Retry_Count()
    {
        var measurements = new ConcurrentBag<(string Name, double Value)>();
        using var listener = CreateMeterListener(measurements);

        await TestPipelineHostBuilder.Create()
            .AddModule<RetriedModule>()
            .ExecutePipelineAsync();

        using (Assert.Multiple())
        {
            await Assert.That(measurements.Any(measurement =>
                    measurement.Name == ModuleActivityTracing.ModuleDurationMetric
                    && measurement.Value >= 0))
                .IsTrue();
            await Assert.That(measurements.Single(measurement =>
                    measurement.Name == ModuleActivityTracing.ModuleRetriesMetric).Value)
                .IsEqualTo(2);
        }
    }

    [Test]
    public async Task Failed_Module_Increments_Failure_Counter()
    {
        var measurements = new ConcurrentBag<(string Name, double Value)>();
        using var listener = CreateMeterListener(measurements);

        ModuleActivityTracing.RecordModuleMetrics(
            typeof(RetriedModule),
            "Failed",
            TimeSpan.Zero);

        await Assert.That(measurements.Single(measurement =>
                measurement.Name == ModuleActivityTracing.ModulesFailedMetric).Value)
            .IsEqualTo(1);
    }

    private static ActivityListener CreateActivityListener(ConcurrentBag<Activity> stoppedActivities)
    {
        var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name is
                ModuleActivityTracing.PipelineSourceName or
                ModuleActivityTracing.ModuleSourceName or
                ModuleActivityTracing.CommandSourceName,
            Sample = static (ref ActivityCreationOptions<ActivityContext> _) =>
                ActivitySamplingResult.AllDataAndRecorded,
            ActivityStopped = stoppedActivities.Add,
        };
        ActivitySource.AddActivityListener(listener);
        return listener;
    }

    private static MeterListener CreateMeterListener(
        ConcurrentBag<(string Name, double Value)> measurements)
    {
        var listener = new MeterListener
        {
            InstrumentPublished = (instrument, meterListener) =>
            {
                if (instrument.Meter.Name == ModuleActivityTracing.MeterName)
                {
                    meterListener.EnableMeasurementEvents(instrument);
                }
            },
        };
        listener.SetMeasurementEventCallback<double>((instrument, measurement, _, _) =>
            measurements.Add((instrument.Name, measurement)));
        listener.SetMeasurementEventCallback<long>((instrument, measurement, _, _) =>
            measurements.Add((instrument.Name, measurement)));
        listener.Start();
        return listener;
    }
}
