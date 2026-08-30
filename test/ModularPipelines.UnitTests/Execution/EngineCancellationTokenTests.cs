using ModularPipelines.Events;
using ModularPipelines.Context;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using PipelineEngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Execution;

[TUnit.Core.NotInParallel(nameof(EngineCancellationTokenTests))]
public class EngineCancellationTokenTests : TestBase
{
    private static readonly TimeSpan WaitForCancellationDelay = TimeSpan.FromMilliseconds(100);
    private static TaskCompletionSource AwaitingPendingModuleStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static TaskCompletionSource AwaitingTerminatedModuleStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static TaskCompletionSource PeerModuleStarted = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private static string CommandReadyFile = string.Empty;

    private class BadModule : ThrowingTestModule<bool>
    {
    }

    [ModularPipelines.DependsOn<BadModule>]
    private class Module1 : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.DependsOn<BadModule>]
    private class AlwaysRunBarrierModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();
    }

    [ModularPipelines.DependsOn<AlwaysRunBarrierModule>]
    private class ModuleBehindAlwaysRunBarrier : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private class LongRunningModule : Module<bool>
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task.WaitAsync(cancellationToken);
            return true;
        }
    }

    private class LongRunningModuleWithoutCancellation : Module<bool>
    {
        private readonly TaskCompletionSource<bool> _taskCompletionSource = new();

        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(TimeSpan.FromSeconds(10));

        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await _taskCompletionSource.Task;
            return true;
        }
    }

    private class WaitForAllFailingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await PeerModuleStarted.Task.WaitAsync(cancellationToken);
            throw new InvalidOperationException("Expected test failure");
        }
    }

    private class WaitForAllCompletingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            PeerModuleStarted.TrySetResult();
            await Task.Delay(WaitForCancellationDelay, cancellationToken);
            return true;
        }
    }

    private class RunningCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return await context.Shell.RunAsync(
                new CommandLineToolOptions("pwsh")
                {
                    Arguments =
                    [
                        "-NoProfile",
                        "-Command",
                        "Set-Content -LiteralPath $env:MP_COMMAND_READY_FILE -Value ready; Start-Sleep -Seconds 60",
                    ],
                },
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["MP_COMMAND_READY_FILE"] = CommandReadyFile,
                    },
                    ExecutionTimeout = null,
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
                },
                cancellationToken);
        }
    }

    private class FailAfterCommandStartsModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            while (!File.Exists(CommandReadyFile))
            {
                if (stopwatch.Elapsed > TimeSpan.FromSeconds(10))
                {
                    throw new TimeoutException("The command process did not signal readiness.");
                }

                await Task.Delay(10, cancellationToken);
            }

            throw new InvalidOperationException("Expected test failure");
        }
    }

    [ModularPipelines.DependsOn<WaitForAllCompletingModule>]
    private class WaitForAllPendingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class AwaitingPendingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            AwaitingPendingModuleStarted.TrySetResult();
            var result = await context.GetModule<CancelledBeforeStartModule>();
            return result.ValueOrDefault == true;
        }
    }

    private class CoordinatedFailingModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await AwaitingPendingModuleStarted.Task.WaitAsync(cancellationToken);
            throw new InvalidOperationException("Coordinated failure");
        }
    }

    [ModularPipelines.DependsOn<CoordinatedFailingModule>]
    private class CancelledBeforeStartModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class AwaitingTerminatedModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithAlwaysRun();

        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            AwaitingTerminatedModuleStarted.TrySetResult();
            var result = await context.GetModule<TerminatedBeforeExecutionModule>();
            return result.Status == ModuleStatus.DependencyFailed;
        }
    }

    private class CoordinatedDependencyFailureModule : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await AwaitingTerminatedModuleStarted.Task.WaitAsync(cancellationToken);
            throw new InvalidOperationException("Dependency failure");
        }
    }

    [ModularPipelines.DependsOn<CoordinatedDependencyFailureModule>]
    private class TerminatedBeforeExecutionModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private class StopOnFirstFailingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Stop-on-first failure");
        }
    }

    private class ReadyHookFailingModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.DependsOn<ReadyHookFailingModule>]
    private class ReadyHookDependentModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [ModularPipelines.DependsOn<ReadyHookFailingModule>]
    private class ReadyHookSiblingDependentModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class CoordinatedModuleResultRegistrar : IModuleResultRegistrar, IDisposable
    {
        private readonly ModuleResultRegistrar _inner;
        private readonly HashSet<Type> _registeredDependentTypes = [];
        private readonly ManualResetEventSlim _allDependentsRegistered = new();

        public void Dispose() => _allDependentsRegistered.Dispose();

        public CoordinatedModuleResultRegistrar(
            IModuleResultRegistry resultRegistry,
            Microsoft.Extensions.Logging.ILogger<ModuleResultRegistrar> logger)
        {
            _inner = new ModuleResultRegistrar(resultRegistry, logger);
        }

        public void RegisterTerminatedResult(IModule module, Type moduleType, Exception exception)
        {
            _inner.RegisterTerminatedResult(module, moduleType, exception);
            RecordDependent(moduleType);

            if (moduleType == typeof(ReadyHookFailingModule)
                && !_allDependentsRegistered.Wait(TestHostSettings.DefaultTestTimeout))
            {
                throw new TimeoutException("Dependent failure workers did not win the coordinated race.");
            }
        }

        public void RegisterDependencyFailedResult(IModule module, Type moduleType, Exception exception)
        {
            _inner.RegisterDependencyFailedResult(module, moduleType, exception);
            RecordDependent(moduleType);
        }

        public void RegisterTerminatedResultsForCancelledModules(
            IReadOnlyList<IModule> modules,
            Exception exception)
        {
            _inner.RegisterTerminatedResultsForCancelledModules(modules, exception);

            foreach (var module in modules)
            {
                RecordDependent(module.GetType());
            }
        }

        private void RecordDependent(Type moduleType)
        {
            if (moduleType != typeof(ReadyHookDependentModule)
                && moduleType != typeof(ReadyHookSiblingDependentModule))
            {
                return;
            }

            lock (_registeredDependentTypes)
            {
                _registeredDependentTypes.Add(moduleType);
                if (_registeredDependentTypes.Count == 2)
                {
                    _allDependentsRegistered.Set();
                }
            }
        }
    }

    private sealed class ThrowingReadyHookHandler : IModuleEventHandler
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            if (context.ModuleType == typeof(ReadyHookFailingModule))
            {
                throw new InvalidOperationException("Ready hook failure");
            }

            return Task.CompletedTask;
        }
    }

    private sealed class IndependentlyCancellingReadyHookHandler : IModuleEventHandler
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            if (context.ModuleType == typeof(ReadyHookFailingModule))
            {
                throw new OperationCanceledException("Independent ready hook cancellation");
            }

            return Task.CompletedTask;
        }
    }

    private class StopOnFirstQueuedDependencyModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [ModularPipelines.DependsOn<StopOnFirstQueuedDependencyModule>]
    private class StopOnFirstPendingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    [Test]
    public async Task Dispose_Releases_Global_Event_Subscriptions()
    {
        var weakReference = CreateDisposedEngineCancellationToken();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        await Assert.That(weakReference.IsAlive).IsFalse();
    }

    [Test]
    public async Task First_CancelKeyPress_Cancels_Gracefully_And_Second_Passes_Through()
    {
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());

        var firstInterruptShouldBeSwallowed = engineCancellationToken.HandleCancelKeyPress();
        var secondInterruptShouldBeSwallowed = engineCancellationToken.HandleCancelKeyPress();

        using (Assert.Multiple())
        {
            await Assert.That(firstInterruptShouldBeSwallowed).IsTrue();
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsTrue();
            await Assert.That(secondInterruptShouldBeSwallowed).IsFalse();
        }
    }

    [Test]
    public async Task FailureCancellation_DoesNotCancelNonFailureToken()
    {
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());

        engineCancellationToken.CancelWithException(new InvalidOperationException("module failed"));

        using (Assert.Multiple())
        {
            await Assert.That(engineCancellationToken.Token.IsCancellationRequested).IsTrue();
            await Assert.That(engineCancellationToken.NonFailureCancellationToken.IsCancellationRequested).IsFalse();
        }
    }

    [Test]
    public async Task UserCancellationAfterFailure_CancelsNonFailureToken()
    {
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        engineCancellationToken.CancelWithException(new InvalidOperationException("module failed"));

        engineCancellationToken.CancelWithReason("user cancelled");

        await Assert.That(engineCancellationToken.NonFailureCancellationToken.IsCancellationRequested).IsTrue();
    }

    [Test]
    public async Task Cancel_And_Dispose_Can_Run_Concurrently()
    {
        for (var iteration = 0; iteration < 1_000; iteration++)
        {
            var engineCancellationToken =
                new PipelineEngineCancellationToken(new PrimaryExceptionContainer());

            await Task.WhenAll(
                Task.Run(engineCancellationToken.Cancel),
                Task.Run(engineCancellationToken.Dispose));
        }
    }

    [Test]
    public async Task FailFast_Reports_DependencyFailed_For_Dependent_Module()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<Module1>();

        // This test expects the pipeline to throw when BadModule fails
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
            Concurrency = options.Concurrency with { MaxParallelism = 1 },
        });

        var host = await builder.BuildAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.RunAsync()).Throws<ModuleFailedException>();

        // Results should be registered before the exception is thrown, no delay needed
        var module1Result = resultRegistry.GetResult(typeof(Module1));
        await Assert.That(module1Result).IsNotNull();
        await Assert.That(module1Result!.Status).IsEqualTo(ModuleStatus.DependencyFailed);
        await Assert.That(module1Result.ExceptionOrDefault).IsTypeOf<DependencyFailedException>();
        await Assert.That(((DependencyFailedException) module1Result.ExceptionOrDefault!).FailingModuleName)
            .IsEqualTo(nameof(BadModule));
    }

    [Test]
    public async Task FailFast_Reports_DependencyFailed_When_ReadyHookThrows()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ReadyHookFailingModule>()
            .AddModule<ReadyHookDependentModule>()
            .AddModuleEventHandler<ThrowingReadyHookHandler>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
            Concurrency = options.Concurrency with { MaxParallelism = 1 },
        });

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.RunAsync()).Throws<InvalidOperationException>();

        var dependentResult = resultRegistry.GetResult(typeof(ReadyHookDependentModule));
        await Assert.That(dependentResult).IsNotNull();
        await Assert.That(dependentResult!.Status).IsEqualTo(ModuleStatus.DependencyFailed);
        await Assert.That(dependentResult.ExceptionOrDefault).IsTypeOf<DependencyFailedException>();
        await Assert.That(((DependencyFailedException) dependentResult.ExceptionOrDefault!).FailingModuleName)
            .IsEqualTo(nameof(ReadyHookFailingModule));
    }

    [Test]
    public async Task FailFast_Preserves_Raw_ReadyHook_Failure_When_Dependent_Reports_First()
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services.RemoveAll<IModuleResultRegistrar>();
                services.AddSingleton<IModuleResultRegistrar, CoordinatedModuleResultRegistrar>();
            })
            .AddModule<ReadyHookFailingModule>()
            .AddModule<ReadyHookDependentModule>()
            .AddModule<ReadyHookSiblingDependentModule>()
            .AddModuleEventHandler<ThrowingReadyHookHandler>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.RunAsync()).Throws<InvalidOperationException>();

        foreach (var dependentType in new[]
                 {
                     typeof(ReadyHookDependentModule),
                     typeof(ReadyHookSiblingDependentModule),
                 })
        {
            var dependentResult = resultRegistry.GetResult(dependentType);
            await Assert.That(dependentResult).IsNotNull();
            await Assert.That(dependentResult!.Status).IsEqualTo(ModuleStatus.DependencyFailed);
            await Assert.That(dependentResult.ExceptionOrDefault).IsTypeOf<DependencyFailedException>();
            await Assert.That(((DependencyFailedException) dependentResult.ExceptionOrDefault!).FailingModuleName)
                .IsEqualTo(nameof(ReadyHookFailingModule));
        }
    }

    [Test]
    public async Task Uncancelled_Worker_Token_Is_Not_Expected_Cancellation()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var exception = new OperationCanceledException(cancellationTokenSource.Token);

        await Assert.That(WorkerCancellationClassifier.IsExpected(
                exception,
                cancellationTokenSource.Token))
            .IsFalse();
    }

    [Test]
    public async Task EngineLinked_Limiter_Cancellation_Is_Expected_Before_Worker_Cancellation()
    {
        using var workerCancellationTokenSource = new CancellationTokenSource();
        using var limiterCancellationTokenSource = new CancellationTokenSource();
        limiterCancellationTokenSource.Cancel();
        var limiterCancellation = new OperationCanceledException(
            limiterCancellationTokenSource.Token);

        var normalizedCancellation = ModuleRunner.NormalizeLimiterCancellation(
            limiterCancellation,
            workerCancellationTokenSource.Token,
            limiterCancellationTokenSource.Token);

        await Assert.That(WorkerCancellationClassifier.IsExpected(
                normalizedCancellation,
                workerCancellationTokenSource.Token))
            .IsTrue();
    }

    [Test]
    public async Task Normalized_Limiter_Cancellation_Is_Treated_As_Pipeline_Cancellation()
    {
        using var workerCancellationTokenSource = new CancellationTokenSource();
        using var limiterCancellationTokenSource = new CancellationTokenSource();
        limiterCancellationTokenSource.Cancel();
        var limiterCancellation = new OperationCanceledException(
            limiterCancellationTokenSource.Token);
        var normalizedCancellation = ModuleRunner.NormalizeLimiterCancellation(
            limiterCancellation,
            workerCancellationTokenSource.Token,
            limiterCancellationTokenSource.Token);

        await Assert.That(ModuleRunner.IsPipelineCancellation(
                normalizedCancellation,
                workerCancellationTokenSource.Token,
                isEngineCancelled: false))
            .IsTrue();
    }

    [Test]
    public async Task FailFast_Wraps_Independent_ReadyHook_Cancellation_For_Dependents()
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigureServices(services =>
            {
                services.RemoveAll<IModuleResultRegistrar>();
                services.AddSingleton<IModuleResultRegistrar, CoordinatedModuleResultRegistrar>();
            })
            .AddModule<ReadyHookFailingModule>()
            .AddModule<ReadyHookDependentModule>()
            .AddModule<ReadyHookSiblingDependentModule>()
            .AddModuleEventHandler<IndependentlyCancellingReadyHookHandler>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.RunAsync()).Throws<OperationCanceledException>();

        foreach (var dependentType in new[]
                 {
                     typeof(ReadyHookDependentModule),
                     typeof(ReadyHookSiblingDependentModule),
                 })
        {
            var dependentResult = resultRegistry.GetResult(dependentType);
            await Assert.That(dependentResult).IsNotNull();
            await Assert.That(dependentResult!.Status).IsEqualTo(ModuleStatus.DependencyFailed);
            await Assert.That(dependentResult.ExceptionOrDefault).IsTypeOf<DependencyFailedException>();
            await Assert.That(((DependencyFailedException) dependentResult.ExceptionOrDefault!).FailingModuleName)
                .IsEqualTo(nameof(ReadyHookFailingModule));
        }
    }

    [Test]
    public async Task FailFast_DoesNotPropagateDependencyFailureAcrossAlwaysRunModule()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<AlwaysRunBarrierModule>()
            .AddModule<ModuleBehindAlwaysRunBarrier>();
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
            Concurrency = options.Concurrency with { MaxParallelism = 1 },
        });

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        await Assert.That(async () => await host.RunAsync()).Throws<ModuleFailedException>();

        var alwaysRunResult = resultRegistry.GetResult(typeof(AlwaysRunBarrierModule));
        var downstreamResult = resultRegistry.GetResult(typeof(ModuleBehindAlwaysRunBarrier));
        using (Assert.Multiple())
        {
            await Assert.That(alwaysRunResult).IsNotNull();
            await Assert.That(alwaysRunResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(downstreamResult).IsNotNull();
            await Assert.That(downstreamResult!.Status).IsEqualTo(ModuleStatus.Cancelled);
        }
    }

    [Test]
    public async Task ContinueOnFailure_Reports_DependencyFailed_For_Dependent_Module()
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                FailureMode = FailureMode.ContinueOnFailure,
            })
            .AddModule<BadModule>()
            .AddModule<Module1>();

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var summary = await host.RunAsync();
        var dependentResult = resultRegistry.GetResult(typeof(Module1));
        var dependentTimeline = summary.ModuleTimelines!.Single(x => x.ModuleName == nameof(Module1));

        using (Assert.Multiple())
        {
            await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(dependentResult).IsNotNull();
            await Assert.That(dependentResult!.Status).IsEqualTo(ModuleStatus.DependencyFailed);
            await Assert.That(dependentTimeline.Status).IsEqualTo(ModuleStatus.DependencyFailed);
            await Assert.That(dependentResult.ExceptionOrDefault).IsTypeOf<DependencyFailedException>();
            await Assert.That(((DependencyFailedException) dependentResult.ExceptionOrDefault!).FailingModuleName)
                .IsEqualTo(nameof(BadModule));
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModule>();

        // This test expects the pipeline to throw when BadModule fails
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
        });

        var host = await builder.BuildAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.RunAsync();

        await Task.Delay(WaitForCancellationDelay);

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(async () => await pipelineTask);

        var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModule));
        await Assert.That(exception).IsNotNull();
        await Assert.That(longRunningModuleResult).IsNotNull();
        await Assert.That(longRunningModuleResult!.Status).IsEqualTo(ModuleStatus.Cancelled);
        await Assert.That(longRunningModuleResult.Duration).IsLessThan(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task Running_Command_Is_PipelineTerminated_When_Engine_Cancels()
    {
        CommandReadyFile = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-command-ready-{Guid.NewGuid():N}");

        try
        {
            var builder = TestPipelineBuilder.Create()
                .AddModule<RunningCommandModule>()
                .AddModule<FailAfterCommandStartsModule>();
            builder.ConfigurePipelineOptions(options => options with
            {
                ThrowOnPipelineFailure = true,
            });

            var host = await builder.BuildAsync();
            var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

            await Assert.ThrowsAsync<ModuleFailedException>(async () => await host.RunAsync());

            var commandResult = resultRegistry.GetResult(typeof(RunningCommandModule));
            await Assert.That(commandResult).IsNotNull();
            await Assert.That(commandResult!.Status).IsEqualTo(ModuleStatus.Cancelled);
        }
        finally
        {
            File.Delete(CommandReadyFile);
        }
    }

    [Test]
    public async Task When_Cancel_Engine_Token_Without_DependsOn_Then_Modules_Cancel_Without_Cancellation()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<BadModule>()
            .AddModule<LongRunningModuleWithoutCancellation>();

        // This test expects the pipeline to throw when BadModule fails
        builder.ConfigurePipelineOptions(options => options with
        {
            ThrowOnPipelineFailure = true,
        });

        var host = await builder.BuildAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var pipelineTask = host.RunAsync();

        await Task.Delay(WaitForCancellationDelay);

        await Assert.That(async () => await pipelineTask).ThrowsException();

        var longRunningModuleResult = resultRegistry.GetResult(typeof(LongRunningModuleWithoutCancellation));
        await Assert.That(longRunningModuleResult).IsNotNull();
        await Assert.That(longRunningModuleResult!.Status).IsEqualTo(ModuleStatus.Cancelled);
    }

    [Test]
    public async Task CancelledBeforeStartModule_UnblocksRuntimeAwaiter()
    {
        AwaitingPendingModuleStarted =
            new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                DefaultModuleTimeout = TimeSpan.Zero,
                ThrowOnPipelineFailure = true,
                Concurrency = options.Concurrency with
                {
                    MaxParallelism = 2,
                },
            })
            .AddModule<AwaitingPendingModule>()
            .AddModule<CoordinatedFailingModule>()
            .AddModule<CancelledBeforeStartModule>();

        var host = await builder.BuildAsync();
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await host.RunAsync().WaitAsync(TestHostSettings.DefaultTestTimeout));

        await Assert.That(exception!.InnerException).IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.InnerException!).HasMessageEqualTo("Coordinated failure");
    }

    [Test]
    public async Task AlwaysRunModuleAwaitingTerminatedModule_UnblocksRuntimeAwaiter()
    {
        AwaitingTerminatedModuleStarted =
            new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                DefaultModuleTimeout = TimeSpan.Zero,
                ThrowOnPipelineFailure = true,
                Concurrency = options.Concurrency with
                {
                    MaxParallelism = 2,
                },
            })
            .AddModule<AwaitingTerminatedModule>()
            .AddModule<CoordinatedDependencyFailureModule>()
            .AddModule<TerminatedBeforeExecutionModule>();

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await host.RunAsync().WaitAsync(TestHostSettings.DefaultTestTimeout));
        var alwaysRunResult = resultRegistry.GetResult<bool>(typeof(AwaitingTerminatedModule));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.InnerException).IsTypeOf<InvalidOperationException>();
            await Assert.That(exception.InnerException!).HasMessageEqualTo("Dependency failure");
            await Assert.That(alwaysRunResult).IsNotNull();
            await Assert.That(alwaysRunResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(alwaysRunResult.ValueOrDefault).IsTrue();
        }
    }

    [Test]
    public async Task FailFast_PendingModuleAwaiterReturnsTerminatedResult()
    {
        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions(options => options with
            {
                DefaultModuleTimeout = TimeSpan.Zero,
                ThrowOnPipelineFailure = true,
                Concurrency = options.Concurrency with
                {
                    MaxParallelism = 1,
                },
            })
            .AddModule<StopOnFirstFailingModule>()
            .AddModule<StopOnFirstQueuedDependencyModule>()
            .AddModule<StopOnFirstPendingModule>();

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var pendingModule = host.Services.GetServices<IModule>()
            .OfType<StopOnFirstPendingModule>()
            .Single();

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await host.RunAsync().WaitAsync(TestHostSettings.DefaultTestTimeout));
        var awaitedResult = await ((IInternalModule) pendingModule).ResultTask
            .WaitAsync(TimeSpan.FromSeconds(1));
        var registeredResult = resultRegistry.GetResult(typeof(StopOnFirstPendingModule));

        using (Assert.Multiple())
        {
            await Assert.That(awaitedResult).IsSameReferenceAs(registeredResult);
            await Assert.That(awaitedResult.Status).IsEqualTo(ModuleStatus.Cancelled);
            await Assert.That(awaitedResult.ExceptionOrDefault)
                .IsSameReferenceAs(exception);
        }
    }

    [Test]
    public async Task ContinueOnFailure_Allows_InFlight_And_Pending_Modules_To_Complete()
    {
        PeerModuleStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options with
            {
                FailureMode = FailureMode.ContinueOnFailure,
            })
            .AddModule<WaitForAllFailingModule>()
            .AddModule<WaitForAllCompletingModule>()
            .AddModule<WaitForAllPendingModule>();

        var host = await builder.BuildAsync();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var pipelineSummary = await host.RunAsync();

        var completingModuleResult = resultRegistry.GetResult(typeof(WaitForAllCompletingModule));
        var pendingModuleResult = resultRegistry.GetResult(typeof(WaitForAllPendingModule));
        await Assert.That(pipelineSummary.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(completingModuleResult).IsNotNull();
        await Assert.That(completingModuleResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(pendingModuleResult).IsNotNull();
        await Assert.That(pendingModuleResult!.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task ContinueOnFailure_Preserves_Original_Failure()
    {
        PeerModuleStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        var builder = TestPipelineBuilder.Create()
            .ConfigurePipelineOptions((_, options) => options with
            {
                FailureMode = FailureMode.ContinueOnFailure,
                ThrowOnPipelineFailure = true,
            })
            .AddModule<WaitForAllFailingModule>()
            .AddModule<WaitForAllCompletingModule>();

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(
            async () => await builder.RunAsync());

        await Assert.That(exception!.InnerException).IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.InnerException!).HasMessageEqualTo("Expected test failure");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateDisposedEngineCancellationToken()
    {
        var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var weakReference = new WeakReference(engineCancellationToken);

        engineCancellationToken.Dispose();

        return weakReference;
    }
}
