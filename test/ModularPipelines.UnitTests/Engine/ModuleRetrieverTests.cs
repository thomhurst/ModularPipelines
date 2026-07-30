using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleRetrieverTests
{
    [Test]
    public async Task EstimatedTimeLookups_AreNotRateLimited()
    {
        var modules = Enumerable.Range(0, 101)
            .Select(_ => Mock.Of<IModule>())
            .ToArray();
        var conditionHandler = new Mock<IModuleConditionHandler>();
        conditionHandler
            .Setup(x => x.ShouldIgnore(It.IsAny<IModule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var registrationEventExecutor = new Mock<IRegistrationEventExecutor>();
        registrationEventExecutor
            .Setup(x => x.InvokeRegistrationEventsAsync(It.IsAny<IEnumerable<IModule>>()))
            .Returns(Task.CompletedTask);
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        var releaseLookups = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var allLookupsStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var startedLookupCount = 0;
        estimatedTimeProvider
            .Setup(x => x.GetModuleEstimatedTimeAsync(It.IsAny<Type>()))
            .Returns(async () =>
            {
                if (Interlocked.Increment(ref startedLookupCount) == modules.Length)
                {
                    allLookupsStarted.TrySetResult();
                }

                await releaseLookups.Task;
                return TimeSpan.Zero;
            });
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(It.IsAny<Type>()))
            .ReturnsAsync([]);
        var retriever = new ModuleRetriever(
            conditionHandler.Object,
            registrationEventExecutor.Object,
            modules,
            estimatedTimeProvider.Object,
            Mock.Of<IModuleDependencyRegistry>(),
            Mock.Of<IModuleMetadataRegistry>(),
            Mock.Of<IDependencyChainProvider>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        var organizedModulesTask = retriever.GetOrganizedModules();
        var allStartedBeforeRelease = await Task.WhenAny(
            allLookupsStarted.Task,
            Task.Delay(TimeSpan.FromSeconds(2))) == allLookupsStarted.Task;
        releaseLookups.TrySetResult();
        await organizedModulesTask;

        await Assert.That(allStartedBeforeRelease).IsTrue();
    }
}
