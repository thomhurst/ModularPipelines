using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.ExecutionBackend.TestFixtures;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class ExecutionBackendTests
{
    [Test]
    public async Task RepeatedExecutionRequestsShareResultAndCompleteScopeDisposal()
    {
        var backend = new CallbackBackend(async (modules, context, cancellationToken) =>
        {
            var first = context.ExecuteModuleAsync(modules.Single(), cancellationToken);
            var second = context.ExecuteModuleAsync(modules.Single(), cancellationToken);
            var results = await Task.WhenAll(first, second);
            await Assert.That(results[0]).IsSameReferenceAs(results[1]);
            await Assert.That(((ScopedBackendModule) modules.Single()).Probe!.Disposed).IsTrue();
            return [results[0]];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<ScopedBackendModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend).AddScoped<ScopeProbe>())
            .BuildAsync();

        await pipeline.RunAsync();
        var module = pipeline.Services.GetServices<IModule>().OfType<ScopedBackendModule>().Single();
        await Assert.That(module.ExecutionCount).IsEqualTo(1);
    }

    [Test]
    public async Task ContextRejectsForeignModulesAndRequestsAfterBackendCompletion()
    {
        IExecutionBackendContext? savedContext = null;
        IModule? plannedModule = null;
        var backend = new CallbackBackend(async (modules, context, cancellationToken) =>
        {
            savedContext = context;
            plannedModule = modules.Single();
            await Assert.That(() => context.ExecuteModuleAsync(new BackendTestModule(), cancellationToken))
                .Throws<ArgumentException>();
            return [await context.ExecuteModuleAsync(plannedModule, cancellationToken)];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await pipeline.RunAsync();
        await Assert.That(() => savedContext!.ExecuteModuleAsync(plannedModule!))
            .Throws<ObjectDisposedException>();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task CancellingExecutionWhileWaitingForDependencyCompletesRequest(
        bool alwaysRun, CancellationToken cancellationToken)
    {
        using var requestCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var requestStarted = false;
        var backend = new CallbackBackend(async (modules, context, _) =>
        {
            var dependent = modules.OfType<OrderingDependentModule>().Single();
            var execution = context.ExecuteModuleAsync(dependent, requestCancellation.Token);
            requestStarted = true;
            requestCancellation.Cancel();
            return [await execution];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddModule(new OrderingDependentModule { AlwaysRun = alwaysRun })
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await Assert.That(() => pipeline.RunAsync(cancellationToken)).Throws<OperationCanceledException>();
        await Assert.That(requestStarted).IsTrue();
        var dependent = pipeline.Services.GetServices<IModule>().OfType<OrderingDependentModule>().Single();
        await Assert.That(dependent.ExecutionCount).IsEqualTo(0);
    }

    [Test]
    public async Task ExternalBackendExecutesModuleThroughEngineLifecycle()
    {
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddExecutionBackend<InProcessExecutionBackend>()
            .BuildAsync();

        var summary = await pipeline.RunAsync();
        var module = pipeline.Services.GetServices<IModule>().OfType<BackendTestModule>().Single();
        var result = await module;

        await Assert.That(result.Value).IsEqualTo(42);
        await Assert.That(module.ExecutionCount).IsEqualTo(1);
        await Assert.That(summary.Modules).Count().IsEqualTo(1);
    }

    [Test]
    public async Task CustomBackendOverridesDistributedBackend()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddDistributedMode(options =>
            {
                options.TotalInstances = 2;
                options.RunId = "backend-test-run";
            })
            .AddExecutionBackend<RecordingExecutionBackend>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IExecutionBackend>())
            .IsTypeOf<RecordingExecutionBackend>();
    }

    [Test]
    public async Task CustomBackendReceivesPlanAndCompletesModule()
    {
        var module = new BackendTestModule();
        var builder = TestPipelineBuilder.Create()
            .AddModule(module)
            .AddExecutionBackend<RecordingExecutionBackend>();
        await using var pipeline = await builder.BuildAsync();

        var backend = pipeline.Services.GetRequiredService<IExecutionBackend>();
        var summary = await pipeline.RunAsync();
        var result = await module;

        using (Assert.Multiple())
        {
            await Assert.That(backend).IsTypeOf<RecordingExecutionBackend>();
            await Assert.That(((RecordingExecutionBackend) backend).ReceivedModules).Count().IsEqualTo(1);
            await Assert.That(((RecordingExecutionBackend) backend).ReceivedModules.Single())
                .IsSameReferenceAs(module);
            await Assert.That(result.Value).IsEqualTo(42);
            await Assert.That(module.ExecutionCount).IsEqualTo(0);
            await Assert.That(summary.Modules).Count().IsEqualTo(1);
        }
    }

    [Test]
    public async Task BackendContextAppliesResultIdempotently()
    {
        var module = new BackendTestModule();
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule(module)
            .BuildAsync();
        var context = pipeline.Services.GetRequiredService<IExecutionBackendContext>();
        var resultRegistry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        var result = CreateResult(module);
        var conflictingResult = CreateResult(module, 43);

        var firstApplication = context.TryApplyResult(module, result);
        var secondApplication = context.TryApplyResult(module, conflictingResult);

        using (Assert.Multiple())
        {
            await Assert.That(firstApplication).IsTrue();
            await Assert.That(secondApplication).IsFalse();
            await Assert.That(await module).IsSameReferenceAs(result);
            await Assert.That(resultRegistry.GetResult(module.GetType())).IsSameReferenceAs(result);
        }
    }

    [Test]
    public async Task BackendContextRegistersAnAlreadyAppliedModuleResult()
    {
        var module = new BackendTestModule();
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule(module)
            .BuildAsync();
        var context = pipeline.Services.GetRequiredService<IExecutionBackendContext>();
        var resultRegistry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        var result = CreateResult(module);
        ModuleCompletionSourceApplicator.TryApply(module, result);

        var applied = context.TryApplyResult(module, result);

        using (Assert.Multiple())
        {
            await Assert.That(applied).IsFalse();
            await Assert.That(resultRegistry.GetResult(module.GetType())).IsSameReferenceAs(result);
        }
    }

    [Test]
    public async Task BackendContextDoesNotRegisterResultWhenModuleAwaitableIsFaulted()
    {
        var module = new BackendTestModule();
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule(module)
            .BuildAsync();
        var context = pipeline.Services.GetRequiredService<IExecutionBackendContext>();
        var resultRegistry = pipeline.Services.GetRequiredService<IModuleResultRegistry>();
        var result = CreateResult(module);
        var failure = new InvalidOperationException("Concurrent module failure");
        module.CompletionSource.TrySetException(failure);

        var applied = context.TryApplyResult(module, result);

        using (Assert.Multiple())
        {
            await Assert.That(applied).IsFalse();
            await Assert.That(resultRegistry.GetResult(module.GetType())).IsNull();
            await Assert.That(module.CompletionSource.Task.Exception?.InnerException)
                .IsSameReferenceAs(failure);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task RemoteDependencyResultUnblocksLocalExecution(
        bool applyBeforeExecution, CancellationToken cancellationToken)
    {
        var backend = new CallbackBackend(async (modules, context, _) =>
        {
            var dependency = modules.OfType<BackendTestModule>().Single();
            var dependent = modules.OfType<DependentBackendModule>().Single();
            if (applyBeforeExecution)
            {
                await Assert.That(context.TryApplyResult(dependency, CreateResult(dependency))).IsTrue();
            }

            var execution = context.ExecuteModuleAsync(dependent, cancellationToken);
            if (!applyBeforeExecution)
            {
                await Assert.That(execution.IsCompleted).IsFalse();
                await Assert.That(context.TryApplyResult(dependency, CreateResult(dependency))).IsTrue();
            }

            var result = await execution;
            await Assert.That(((ModuleResult<int>) result).Value).IsEqualTo(43);
            await Assert.That(dependency.ExecutionCount).IsEqualTo(0);
            await Assert.That(dependent.ExecutionCount).IsEqualTo(1);
            return [result];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddModule<DependentBackendModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await pipeline.RunAsync(cancellationToken);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    [Timeout(30_000)]
    public async Task IgnoredRemoteFailureUnblocksLocalDependency(
        bool applyBeforeExecution, CancellationToken cancellationToken)
    {
        var backend = new CallbackBackend(async (modules, context, _) =>
        {
            var dependency = modules.OfType<BackendTestModule>().Single();
            var dependent = modules.OfType<OrderingDependentModule>().Single();
            var remoteFailure = ModuleResult<int>.CreateFailure(
                new InvalidOperationException("Ignored remote failure"),
                new ModuleExecutionContext(dependency, dependency.GetType())) with
            {
                Status = ModuleStatus.FailureIgnored,
            };
            if (applyBeforeExecution)
            {
                await Assert.That(context.TryApplyResult(dependency, remoteFailure)).IsTrue();
            }

            var execution = context.ExecuteModuleAsync(dependent, cancellationToken);
            if (!applyBeforeExecution)
            {
                await Assert.That(execution.IsCompleted).IsFalse();
                await Assert.That(context.TryApplyResult(dependency, remoteFailure)).IsTrue();
            }

            var result = await execution;
            await Assert.That(((ModuleResult<int>) result).Value).IsEqualTo(43);
            await Assert.That(dependency.ExecutionCount).IsEqualTo(0);
            await Assert.That(dependent.ExecutionCount).IsEqualTo(1);
            await Assert.That(await dependency).IsSameReferenceAs(remoteFailure);
            return [result];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<BackendTestModule>()
            .AddModule<OrderingDependentModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await pipeline.RunAsync(cancellationToken);
    }

    [Test]
    [Arguments(ModuleStatus.Failed)]
    [Arguments(ModuleStatus.TimedOut)]
    [Arguments(ModuleStatus.Cancelled)]
    [Arguments(ModuleStatus.DependencyFailed)]
    [Timeout(30_000)]
    public async Task RemoteFailurePreventsLocalExecution(ModuleStatus status, CancellationToken cancellationToken)
    {
        var dependency = new BackendTestModule();
        var dependent = new OrderingDependentModule();
        var backend = new CallbackBackend(async (_, context, _) =>
        {
            var result = ModuleResult<int>.CreateFailure(
                new InvalidOperationException("Remote failure"),
                new ModuleExecutionContext(dependency, dependency.GetType())) with
            { Status = status };
            await Assert.That(context.TryApplyResult(dependency, result)).IsTrue();
            return [await context.ExecuteModuleAsync(dependent, cancellationToken)];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule(dependency)
            .AddModule(dependent)
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend))
            .BuildAsync();

        await Assert.That(() => pipeline.RunAsync(cancellationToken)).Throws<DependencyFailedException>();
        await Assert.That(dependent.ExecutionCount).IsEqualTo(0);
    }

    [Test]
    [Timeout(30_000)]
    public async Task ExecutionRemainsActiveUntilScopeDisposalCompletes(CancellationToken cancellationToken)
    {
        var allowDisposal = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var probe = new ScopeProbe { AllowDisposal = allowDisposal.Task };
        var backend = new CallbackBackend(async (modules, context, _) =>
        {
            var module = modules.Single();
            var execution = context.ExecuteModuleAsync(module, cancellationToken);
            try
            {
                await probe.DisposalStarted.Task.WaitAsync(cancellationToken);
                await Assert.That(execution.IsCompleted).IsFalse();
                await Assert.That(() => context.TryApplyResult(module, CreateResult(module)))
                    .Throws<InvalidOperationException>();

                using var waitCancellation = new CancellationTokenSource();
                var duplicate = context.ExecuteModuleAsync(module, waitCancellation.Token);
                waitCancellation.Cancel();
                await Assert.That(async () => await duplicate).Throws<OperationCanceledException>();
                await Assert.That(execution.IsCompleted).IsFalse();
            }
            finally
            {
                allowDisposal.TrySetResult();
            }

            var result = await execution;
            await Assert.That(probe.Disposed).IsTrue();
            return [result];
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule<ScopedBackendModule>()
            .ConfigureServices(services => services.AddSingleton<IExecutionBackend>(backend).AddScoped(_ => probe))
            .BuildAsync();

        await pipeline.RunAsync(cancellationToken);
    }
    private static ModuleResult<int> CreateResult(IModule module, int value = 42)
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<int>.Success(value)
        {
            Name = module.GetType().Name,
            TypeName = module.GetType().FullName,
            StartTime = now,
            EndTime = now,
            Duration = TimeSpan.Zero,
            Status = ModuleStatus.Succeeded,
        };
    }

    private sealed class RecordingExecutionBackend : IExecutionBackend
    {
        public IReadOnlyList<IModule> ReceivedModules { get; private set; } = [];

        public bool OwnsEntirePlan => true;

        public Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
            IReadOnlyList<IModule> modules,
            IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
            IExecutionBackendContext context,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ReceivedModules = modules;
            var result = CreateResult(modules.Single());
            return Task.FromResult<IReadOnlyList<IModuleResult>>([result]);
        }
    }

    private sealed class BackendTestModule : Module<int>
    {
        public int ExecutionCount { get; private set; }

        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return Task.FromResult(42);
        }
    }

    [ModularPipelines.DependsOn<BackendTestModule>]
    private sealed class DependentBackendModule : Module<int>
    {
        public int ExecutionCount { get; private set; }

        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return (await context.GetModule<BackendTestModule>()).Value + 1;
        }
    }

    [ModularPipelines.DependsOn<BackendTestModule>]
    private sealed class OrderingDependentModule : Module<int>
    {
        public bool AlwaysRun { get; init; }

        public int ExecutionCount { get; private set; }

        protected override void Configure(ModuleConfigurationBuilder module)
        {
            if (AlwaysRun)
            {
                module.WithAlwaysRun();
            }
        }

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            return Task.FromResult(43);
        }
    }

    private sealed class ScopedBackendModule : Module<int>
    {
        public ScopeProbe? Probe { get; private set; }

        public int ExecutionCount { get; private set; }

        protected internal override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Probe = context.Services.GetRequiredService<ScopeProbe>();
            ExecutionCount++;
            await Task.Yield();
            return 42;
        }
    }

    private sealed class ScopeProbe : IAsyncDisposable
    {
        public bool Disposed { get; private set; }

        public TaskCompletionSource DisposalStarted { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public Task? AllowDisposal { get; init; }

        public async ValueTask DisposeAsync()
        {
            DisposalStarted.TrySetResult();
            if (AllowDisposal is not null)
            {
                await AllowDisposal;
            }

            Disposed = true;
        }
    }

    private sealed class CallbackBackend(
        Func<IReadOnlyList<IModule>, IExecutionBackendContext, CancellationToken, Task<IReadOnlyList<IModuleResult>>> callback)
        : IExecutionBackend
    {
        public bool OwnsEntirePlan => true;

        public Task<IReadOnlyList<IModuleResult>> ExecuteAsync(
            IReadOnlyList<IModule> modules,
            IReadOnlyDictionary<Type, TimeSpan> estimatedDurations,
            IExecutionBackendContext context,
            CancellationToken cancellationToken) => callback(modules, context, cancellationToken);
    }
}
