using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Constants;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.Tracing;
using Moq;

namespace ModularPipelines.UnitTests.Tracing;

[NotInParallel]
public class TelemetryIntegrationTests
{
    private const string Secret = "telemetry-secret-value";
    private const string SecretTool = "telemetry-" + Secret + "-tool";
    private const string ObfuscatedTool = "telemetry-**********-tool";
    private const string UnregisteredSensitiveArgument = "unregistered-sensitive-argument";
    private static int _inputManipulatorInvocations;
    private static int _throwingInputManipulatorInvocations;

    private sealed class SuccessfulCommandInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult<CommandResult?>(CommandResult.Ok());
        }
    }

    private sealed class ThrowingCommandInterceptor : ICommandInterceptor
    {
        public ValueTask<CommandResult?> InterceptAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException($"Telemetry failure contains {Secret}");
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
                new GenericCommandLineToolOptions(SecretTool)
                {
                    Arguments = [Secret],
                },
                cancellationToken: cancellationToken);
        }
    }

    private sealed class HiddenArgumentsCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("hidden-arguments-tool")
                {
                    Arguments = [UnregisteredSensitiveArgument],
                },
                new CommandExecutionOptions
                {
                    LogSettings = new CommandLoggingOptions { ShowCommandArguments = false },
                },
                cancellationToken);
        }
    }

    private sealed class DefaultLoggingCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("silent-tool")
                {
                    Arguments = [UnregisteredSensitiveArgument],
                },
                cancellationToken: cancellationToken);
        }
    }

    private sealed class ManipulatedInputCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("manipulated-input-tool"),
                new CommandExecutionOptions
                {
                    InputLoggingManipulator = _ =>
                    {
                        Interlocked.Increment(ref _inputManipulatorInvocations);
                        return "manipulated-command-input";
                    },
                },
                cancellationToken);
        }
    }

    private sealed class ThrowingInputManipulatorCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineToolAsync(
                new GenericCommandLineToolOptions("throwing-input-manipulator-tool"),
                new CommandExecutionOptions
                {
                    InputLoggingManipulator = _ =>
                    {
                        Interlocked.Increment(ref _throwingInputManipulatorInvocations);
                        throw new InvalidOperationException("Input manipulator failed");
                    },
                },
                cancellationToken);
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

    private sealed class TimedOutModule : Module<bool>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMilliseconds(10))
            .Build();

        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            return true;
        }
    }

    [Test]
    public async Task Pipeline_Module_And_Command_Activities_Are_Parented_And_Obfuscated()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await builder.AddModule<CommandModule>().ExecutePipelineAsync();

        var pipelineActivity = stoppedActivities.Single(activity => activity.OperationName == "Pipeline.Run");
        var moduleActivity = stoppedActivities.Single(activity => activity.OperationName == $"Module.{nameof(CommandModule)}");
        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == $"Command.{ObfuscatedTool}");
        var commandInput = commandActivity.GetTagItem(ModuleActivityTracing.CommandInputTag)?.ToString();

        using (Assert.Multiple())
        {
            await Assert.That(moduleActivity.ParentSpanId).IsEqualTo(pipelineActivity.SpanId);
            await Assert.That(commandActivity.ParentSpanId).IsEqualTo(moduleActivity.SpanId);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandToolTag))
                .IsEqualTo(ObfuscatedTool);
            await Assert.That(commandActivity.OperationName).DoesNotContain(Secret);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandExitCodeTag))
                .IsEqualTo(0);
            await Assert.That(commandInput).Contains("**********");
            await Assert.That(commandInput).DoesNotContain(Secret);
        }
    }

    [Test]
    public async Task Hidden_Command_Arguments_Are_Not_Exported()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await builder.AddModule<HiddenArgumentsCommandModule>().ExecutePipelineAsync();

        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == "Command.hidden-arguments-tool");
        await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandInputTag))
            .IsEqualTo(LoggingConstants.CommandMask);
    }

    [Test]
    public async Task Silent_Default_Command_Logging_Does_Not_Export_Arguments()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                Commands = options.Commands with
                {
                    Logging = CommandLoggingOptions.Silent,
                },
            });
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await builder.AddModule<DefaultLoggingCommandModule>().ExecutePipelineAsync();

        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == "Command.silent-tool");
        await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandInputTag))
            .IsEqualTo(LoggingConstants.CommandMask);
    }

    [Test]
    public async Task Command_Input_Manipulator_Is_Reused_For_Logging_And_Telemetry()
    {
        _inputManipulatorInvocations = 0;
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await builder.AddModule<ManipulatedInputCommandModule>().ExecutePipelineAsync();

        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == "Command.manipulated-input-tool");
        using (Assert.Multiple())
        {
            await Assert.That(_inputManipulatorInvocations).IsEqualTo(1);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.CommandInputTag))
                .IsEqualTo("manipulated-command-input");
        }
    }

    [Test]
    public async Task Command_Input_Manipulator_Failure_Is_Recorded_On_Activity()
    {
        _throwingInputManipulatorInvocations = 0;
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, SuccessfulCommandInterceptor>();
        await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await builder.AddModule<ThrowingInputManipulatorCommandModule>().ExecutePipelineAsync());

        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == "Command.throwing-input-manipulator-tool");
        using (Assert.Multiple())
        {
            await Assert.That(commandActivity.Status).IsEqualTo(ActivityStatusCode.Error);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.ExceptionTypeTag))
                .IsEqualTo(typeof(InvalidOperationException).FullName);
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.ExceptionMessageTag))
                .IsEqualTo("Input manipulator failed");
            await Assert.That(_throwingInputManipulatorInvocations).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Interceptor_Failure_Precedes_Input_Manipulator()
    {
        _throwingInputManipulatorInvocations = 0;
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, ThrowingCommandInterceptor>();
        await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await builder.AddModule<ThrowingInputManipulatorCommandModule>().ExecutePipelineAsync());

        var commandActivity = stoppedActivities.Single(activity =>
            activity.OperationName == "Command.throwing-input-manipulator-tool");
        using (Assert.Multiple())
        {
            await Assert.That(commandActivity.GetTagItem(ModuleActivityTracing.ExceptionMessageTag))
                .IsEqualTo($"Telemetry failure contains {Secret}");
            await Assert.That(_throwingInputManipulatorInvocations).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Module_Metrics_Record_Duration_And_Retry_Count()
    {
        var measurements = new ConcurrentBag<(string Name, double Value)>();
        using var listener = CreateMeterListener(measurements);

        await TestPipelineBuilder.Create()
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
    public async Task Cache_Outcomes_Record_Metrics_And_Activity_Tags()
    {
        var measurements = new ConcurrentBag<(string Name, double Value)>();
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var meterListener = CreateMeterListener(measurements);
        using var activityListener = CreateActivityListener(stoppedActivities);

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(CommandModule)))
        {
            ModuleActivityTracing.RecordCacheHit(activity, typeof(CommandModule));
            ModuleActivityTracing.RecordCachedResult(activity);
        }

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(RetriedModule)))
        {
            ModuleActivityTracing.RecordCacheMiss(activity, typeof(RetriedModule));
            ModuleActivityTracing.RecordSuccess(activity);
        }

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(TimedOutModule)))
        {
            ModuleActivityTracing.RecordCacheDisabled(activity);
            ModuleActivityTracing.RecordSuccess(activity);
        }

        var hitActivity = stoppedActivities.Single(activity =>
            activity.OperationName == $"Module.{nameof(CommandModule)}");
        var missActivity = stoppedActivities.Single(activity =>
            activity.OperationName == $"Module.{nameof(RetriedModule)}");
        var disabledActivity = stoppedActivities.Single(activity =>
            activity.OperationName == $"Module.{nameof(TimedOutModule)}");

        using (Assert.Multiple())
        {
            await Assert.That(measurements.Single(measurement =>
                    measurement.Name == ModuleActivityTracing.ModuleCacheHitsMetric).Value)
                .IsEqualTo(1);
            await Assert.That(measurements.Single(measurement =>
                    measurement.Name == ModuleActivityTracing.ModuleCacheMissesMetric).Value)
                .IsEqualTo(1);
            await Assert.That(hitActivity.GetTagItem(ModuleActivityTracing.ModuleStatusTag))
                .IsEqualTo("CachedResult");
            await Assert.That(hitActivity.GetTagItem(ModuleActivityTracing.ModuleCacheTag))
                .IsEqualTo("hit");
            await Assert.That(missActivity.GetTagItem(ModuleActivityTracing.ModuleCacheTag))
                .IsEqualTo("miss");
            await Assert.That(disabledActivity.GetTagItem(ModuleActivityTracing.ModuleCacheTag))
                .IsEqualTo("disabled");
        }
    }

    [Test]
    public async Task Cache_Outcomes_Do_Not_Tag_Pipeline_When_Module_Is_Not_Sampled()
    {
        using var listener = new ActivityListener
        {
            ShouldListenTo = source => source.Name == ModuleActivityTracing.PipelineSourceName,
            Sample = static (ref ActivityCreationOptions<ActivityContext> _) =>
                ActivitySamplingResult.AllDataAndRecorded,
        };
        ActivitySource.AddActivityListener(listener);

        using var pipelineActivity = ModuleActivityTracing.StartPipelineActivity("TestPipeline");
        ModuleActivityTracing.RecordCacheHit(activity: null, typeof(CommandModule));

        using (Assert.Multiple())
        {
            await Assert.That(Activity.Current).IsSameReferenceAs(pipelineActivity);
            await Assert.That(pipelineActivity!.GetTagItem(ModuleActivityTracing.ModuleCacheTag))
                .IsNull();
        }
    }

    [Test]
    public async Task Failure_Activities_Obfuscate_Registered_Secrets()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ICommandInterceptor, ThrowingCommandInterceptor>();
        await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await builder.AddModule<CommandModule>().ExecutePipelineAsync());

        var failureActivities = stoppedActivities.Where(activity =>
                activity.OperationName is "Pipeline.Run"
                    or $"Module.{nameof(CommandModule)}"
                    or $"Command.{ObfuscatedTool}")
            .ToArray();

        await Assert.That(failureActivities).Count().IsEqualTo(3);

        foreach (var activity in failureActivities)
        {
            var exceptionMessage = activity.GetTagItem(ModuleActivityTracing.ExceptionMessageTag)?.ToString();
            await Assert.That(exceptionMessage).Contains("**********");
            await Assert.That(exceptionMessage).DoesNotContain(Secret);
            await Assert.That(activity.StatusDescription).Contains("**********");
            await Assert.That(activity.StatusDescription).DoesNotContain(Secret);
        }
    }

    [Test]
    public async Task Thrown_Pipeline_Failure_Preserves_Summary_Status()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);
        var moduleException = new InvalidOperationException("Module failed");
        var failedResult = Mock.Of<IModuleResult>(result =>
            result.ExceptionOrDefault == moduleException
            && result.ModuleStatus == Status.Failed);
        var summary = new PipelineSummary(
            [],
            [failedResult],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        var exception = new PipelineFailedException(summary, ["FailedModule"]);

        using (var activity = ModuleActivityTracing.StartPipelineActivity("TestPipeline"))
        {
            ModuleActivityTracing.RecordPipelineFailure(activity, exception, exception.Message);
        }

        var pipelineActivity = stoppedActivities.Single();
        await Assert.That(pipelineActivity.GetTagItem(ModuleActivityTracing.PipelineStatusTag))
            .IsEqualTo("Failed");
        await Assert.That(pipelineActivity.Status).IsEqualTo(ActivityStatusCode.Error);
    }

    [Test]
    public async Task Canceled_Pipeline_Failures_Are_Tagged_As_Terminated()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);
        using var engineCancellationToken = new ModularPipelines.Engine.EngineCancellationToken(
            Mock.Of<IPrimaryExceptionContainer>());
        var exceptions = new Exception[]
        {
            new OperationCanceledException(),
            new TaskCanceledException(),
            new PipelineCancelledException(engineCancellationToken),
        };

        foreach (var exception in exceptions)
        {
            using var activity = ModuleActivityTracing.StartPipelineActivity("TestPipeline");
            ModuleActivityTracing.RecordPipelineFailure(activity, exception, exception.Message);
        }

        await Assert.That(stoppedActivities).Count().IsEqualTo(exceptions.Length);
        foreach (var activity in stoppedActivities)
        {
            await Assert.That(activity.GetTagItem(ModuleActivityTracing.PipelineStatusTag))
                .IsEqualTo("PipelineTerminated");
            await Assert.That(activity.Status).IsEqualTo(ActivityStatusCode.Error);
        }
    }

    [Test]
    public async Task UsedHistory_Is_Preserved_In_Module_Activity()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(CommandModule)))
        {
            ModuleActivityTracing.RecordUsedHistory(activity);
        }

        var moduleActivity = stoppedActivities.Single();
        await Assert.That(moduleActivity.GetTagItem(ModuleActivityTracing.ModuleStatusTag))
            .IsEqualTo("UsedHistory");
        await Assert.That(moduleActivity.Status).IsEqualTo(ActivityStatusCode.Ok);
    }

    [Test]
    public async Task PipelineTerminated_Is_Preserved_In_Module_Activity()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(CommandModule)))
        {
            ModuleActivityTracing.RecordPipelineTerminated(activity);
        }

        var moduleActivity = stoppedActivities.Single();
        await Assert.That(moduleActivity.GetTagItem(ModuleActivityTracing.ModuleStatusTag))
            .IsEqualTo("PipelineTerminated");
        await Assert.That(moduleActivity.Status).IsEqualTo(ActivityStatusCode.Error);
    }

    [Test]
    public async Task TimedOut_Is_Preserved_In_Module_Activity()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        await Assert.ThrowsAsync<ModuleFailedException>(async () =>
            await TestPipelineBuilder.Create()
                .AddModule<TimedOutModule>()
                .ExecutePipelineAsync());

        var moduleActivity = stoppedActivities.Single(activity =>
            activity.OperationName == $"Module.{nameof(TimedOutModule)}");
        await Assert.That(moduleActivity.GetTagItem(ModuleActivityTracing.ModuleStatusTag))
            .IsEqualTo("TimedOut");
        await Assert.That(moduleActivity.Status).IsEqualTo(ActivityStatusCode.Error);
    }

    [Test]
    public async Task Public_RecordFailure_Does_Not_Export_Raw_Exception_Message()
    {
        var stoppedActivities = new ConcurrentBag<Activity>();
        using var listener = CreateActivityListener(stoppedActivities);

        using (var activity = ModuleActivityTracing.StartModuleActivity(typeof(CommandModule)))
        {
            ModuleActivityTracing.RecordFailure(
                activity,
                new InvalidOperationException($"Failure contains {Secret}"));
        }

        var moduleActivity = stoppedActivities.Single();
        await Assert.That(moduleActivity.GetTagItem(ModuleActivityTracing.ExceptionMessageTag)?.ToString())
            .DoesNotContain(Secret);
        await Assert.That(moduleActivity.StatusDescription).DoesNotContain(Secret);
    }

    [Test]
    [Arguments("Failed")]
    [Arguments("TimedOut")]
    public async Task Failed_Module_Status_Increments_Failure_Counter(string status)
    {
        var measurements = new ConcurrentBag<(string Name, double Value)>();
        using var listener = CreateMeterListener(measurements);

        ModuleActivityTracing.RecordModuleMetrics(
            typeof(RetriedModule),
            status,
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
