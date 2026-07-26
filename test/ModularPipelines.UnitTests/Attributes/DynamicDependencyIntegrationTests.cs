using ModularPipelines.Attributes.Events;
using ModularPipelines.Conditions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

[NotInParallel(nameof(DynamicDependencyIntegrationTests))]
public class DynamicDependencyIntegrationTests : TestBase
{
    private static readonly List<string> ExecutionOrder = new();

    public class AddDependencyAttribute : Attribute, IModuleRegistrationEventReceiver
    {
        private readonly Type _dependencyType;

        public AddDependencyAttribute(Type dependencyType)
        {
            _dependencyType = dependencyType;
        }

        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(_dependencyType);
            return Task.CompletedTask;
        }
    }

    public class ModuleA : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("A");
            await Task.Yield();
            return "A";
        }
    }

    [AddDependency(typeof(ModuleA))]
    public class ModuleB : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("B");
            await Task.Yield();
            return "B";
        }
    }

    public class MissingDynamicDependency : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [AddDependency(typeof(MissingDynamicDependency))]
    public class ModuleWithMissingDynamicDependency : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [AddDependency(typeof(DynamicCycleModuleB))]
    public class DynamicCycleModuleA : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [AddDependency(typeof(DynamicCycleModuleA))]
    public class DynamicCycleModuleB : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    public class AvailableDependency : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [ModularPipelines.Attributes.DependsOn<AvailableDependency>]
    public class RunnableModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    public class RunOnSecondEvaluation : IRunCondition
    {
        private static int _evaluationCount;

        public static void Reset() => _evaluationCount = 0;

        public Task<bool> EvaluateAsync(IPipelineHookContext context)
            => Task.FromResult(Interlocked.Increment(ref _evaluationCount) > 1);
    }

    [ModularPipelines.Attributes.RunIfAll<RunOnSecondEvaluation>]
    public class StatefulRunnableModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOn<MissingDynamicDependency>()
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string?>(null);
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        ExecutionOrder.Clear();
    }

    [Test]
    public async Task DynamicDependency_ModuleBWaitsForModuleA()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(ExecutionOrder).IsEquivalentTo(new[] { "A", "B" });
    }

    [Test]
    public async Task DynamicDependency_MissingModuleFailsBeforeScheduling()
    {
        await Assert.ThrowsAsync<ModuleNotRegisteredException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<ModuleWithMissingDynamicDependency>()
                .ExecutePipelineAsync());
    }

    [Test]
    public async Task DynamicDependency_CycleFailsBeforeScheduling()
    {
        await Assert.ThrowsAsync<DependencyCollisionException>(() =>
            TestPipelineHostBuilder.Create()
                .AddModule<DynamicCycleModuleA>()
                .AddModule<DynamicCycleModuleB>()
                .ExecutePipelineAsync());
    }

    [Test]
    public async Task RuntimeValidation_UsesModulesOutsideValidationTargetAsAvailableDependencies()
    {
        var runnableModule = new RunnableModule();
        var availableDependency = new AvailableDependency();

        await Assert.That(() => ModuleDependencyValidator.Validate(
                [runnableModule],
                [runnableModule, availableDependency],
                dynamicRegistry: null,
                metadataRegistry: null))
            .ThrowsNothing();
    }

    [Test]
    public async Task RuntimeValidation_CatchesDependencyAfterRunConditionChanges()
    {
        RunOnSecondEvaluation.Reset();

        await using var pipeline = TestPipelineHostBuilder.Create()
            .AddModule<StatefulRunnableModule>()
            .Build();

        await Assert.ThrowsAsync<ModuleNotRegisteredException>(() => pipeline.RunAsync());
    }
}
