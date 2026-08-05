using Mediator;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Configuration;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel("ProcessEnvironment")]
public class IgnoredModuleResultRegistrarTests
{
    [Test]
    public async Task Distributed_Worker_Does_Not_Cascade_Local_Skip_To_Dependent()
    {
        var previousInstance = Environment.GetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE");

        try
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", "1");
            var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions
            {
                Enabled = true,
                InstanceIndex = 1,
                TotalInstances = 2,
            });
            var dependency = new ForeignOperatingSystemModule();
            var dependent = new CrossPlatformDependentModule();
            var organizedModules = new OrganizedModules(
                [new RunnableModule(dependent, TimeSpan.Zero)],
                [new IgnoredModule(dependency, SkipDecision.Skip("Unavailable on this worker"))]);
            var contextProvider = new Mock<IPipelineContextProvider>();
            contextProvider
                .Setup(provider => provider.GetModuleContext())
                .Returns(Mock.Of<IPipelineContext>());
            var resultRegistry = new ModuleResultRegistry();
            var registrar = new IgnoredModuleResultRegistrar(
                resultRegistry,
                new ModuleResultHistoryProvider(
                    new NoOpModuleResultRepository(),
                    NullLogger<ModuleResultHistoryProvider>.Instance),
                contextProvider.Object,
                new ModuleDependencyRegistry(),
                Mock.Of<IModuleMetadataRegistry>(),
                options,
                new RoleDetector(options),
                NullLogger<IgnoredModuleResultRegistrar>.Instance,
                new ModulePlanningSkipEvaluator(
                    Mock.Of<IServiceProvider>(),
                    Mock.Of<IModuleConditionHandler>(),
                    Mock.Of<IMediator>(),
                    Mock.Of<ISafeModuleEstimatedTimeProvider>()));

            var result = await registrar.RegisterIgnoredModuleResultsAsync(organizedModules);

            await Assert.That(result.RunnableModules.Select(module => module.Module))
                .Contains(dependent);
            await Assert.That(result.IgnoredModules.Select(module => module.Module))
                .DoesNotContain(dependent);
            await Assert.That(resultRegistry.GetResult(dependency.GetType())).IsNull();
            await Assert.That(((IInternalModule) dependency).ResultTask.IsCompleted).IsFalse();
        }
        finally
        {
            Environment.SetEnvironmentVariable("MODULAR_PIPELINES_INSTANCE", previousInstance);
        }
    }

    [RunIfAll<OnWindows>]
    private sealed class ForeignOperatingSystemModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(string.Empty);
    }

    [ModularPipelines.Attributes.DependsOn<ForeignOperatingSystemModule>]
    private sealed class CrossPlatformDependentModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult(string.Empty);
    }
}
