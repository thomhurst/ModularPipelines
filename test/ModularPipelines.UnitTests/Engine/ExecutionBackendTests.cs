using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Engine;

public class ExecutionBackendTests
{
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
        protected internal override Task<int> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("The custom backend should execute this module.");
        }
    }
}
