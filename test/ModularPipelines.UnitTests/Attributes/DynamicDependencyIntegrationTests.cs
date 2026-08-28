using ModularPipelines.Attributes.Events;
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
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("A");
            await Task.Yield();
            return "A";
        }
    }

    [AddDependency(typeof(ModuleA))]
    public class ModuleB : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionOrder.Add("B");
            await Task.Yield();
            return "B";
        }
    }

    // Never registered — a dynamic dependency on this module cannot be satisfied.
    public class UnregisteredModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "unregistered";
        }
    }

    // The dependency is added at registration time (after build-time auto-registration has run),
    // so nothing auto-registers UnregisteredModule and it must be caught by the run-time revalidation.
    [AddDependency(typeof(UnregisteredModule))]
    public class ModuleWithMissingDynamicDependency : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "never runs";
        }
    }

    [ModularPipelines.Attributes.ModuleCategory("compile")]
    public class DynamicallySkippedDependency : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A filtered dependency must not execute");
    }

    [ModularPipelines.Attributes.ModuleCategory("test")]
    [AddDependency(typeof(DynamicallySkippedDependency))]
    public class DynamicallySkippedDependent : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("A cascade-skipped dependent must not execute");
    }

    [Before(Test)]
    public void ClearExecutionOrder()
    {
        ExecutionOrder.Clear();
    }

    [Test]
    public async Task DynamicDependency_ModuleBWaitsForModuleA()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<ModuleA>()
            .AddModule<ModuleB>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
        await Assert.That(ExecutionOrder).IsEquivalentTo(new[] { "A", "B" });
    }

    [Test]
    public async Task DynamicDependency_OnUnregisteredModule_FailsFast()
    {
        // A dependency added via a registration event on a module that is not registered
        // (and cannot be auto-registered, because it is not declared via [DependsOn]) must be
        // caught by the run-time revalidation of the canonical graph, before the scheduler runs,
        // rather than surfacing late as a dependency-waiter failure.
        await Assert.ThrowsAsync<ModuleNotRegisteredException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<ModuleWithMissingDynamicDependency>()
                .RunAsync());
    }

    [Test]
    public async Task DynamicDependency_OnFilteredModule_CascadeSkipsDependent()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<DynamicallySkippedDependency>()
            .AddModule<DynamicallySkippedDependent>()
            .ConfigurePipelineOptions(options => options with { RunOnlyCategories = ["test"] })
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);

        var dependentResult = await result.Modules
            .OfType<DynamicallySkippedDependent>()
            .Single();
        await Assert.That(dependentResult.SkipDecisionOrDefault).IsNotNull();
        await Assert.That(dependentResult.SkipDecisionOrDefault!.Reason)
            .Contains(nameof(DynamicallySkippedDependency));
    }
}
