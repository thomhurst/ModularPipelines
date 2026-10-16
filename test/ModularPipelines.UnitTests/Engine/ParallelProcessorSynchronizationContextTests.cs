using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Requirements;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ParallelProcessorSynchronizationContextTests
{
    [Test]
    public async Task RequirementChecking_DoesNotCaptureSynchronizationContext()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var requirement = new Mock<IPipelineRequirement>();
        requirement.Setup(x => x.EvaluateAsync(It.IsAny<IPipelineContext>(), It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await release.Task.ConfigureAwait(false);
                return RequirementDecision.Passed;
            });
        var checker = new RequirementChecker([requirement.Object], CreateContextProvider());

        await AssertNoContextCapture(() => checker.CheckRequirementsAsync(CancellationToken.None), release);
    }

    [Test]
    public async Task ModuleRetrieval_DoesNotCaptureSynchronizationContext()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var conditionHandler = new Mock<IModuleConditionHandler>();
        conditionHandler.Setup(x => x.ShouldIgnoreByCategory(It.IsAny<IModule>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, null));
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        estimatedTimeProvider.Setup(x => x.GetModuleEstimatedTimeAsync(It.IsAny<Type>(), It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                await release.Task.ConfigureAwait(false);
                return TimeSpan.Zero;
            });
        var retriever = new ModuleRetriever(
            conditionHandler.Object,
            Mock.Of<IRegistrationEventExecutor>(),
            [Mock.Of<IModule>()],
            estimatedTimeProvider.Object,
            Mock.Of<IModuleDependencyRegistry>(),
            Mock.Of<IModuleMetadataRegistry>(),
            Mock.Of<IDependencyChainProvider>(),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));

        await AssertNoContextCapture(() => retriever.GetOrganizedModules(), release);
    }

    [Test]
    public async Task PipelineFileWriting_DoesNotCaptureSynchronizationContext()
    {
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var writer = new Mock<IBuildSystemPipelineFileWriter>();
        writer.Setup(x => x.WriteAsync(It.IsAny<IPipelineContext>())).Returns(release.Task);
        using var services = new ServiceCollection()
            .AddSingleton(writer.Object)
            .BuildServiceProvider();
        var fileWriter = new PipelineFileWriter(services, CreateContextProvider());

        try
        {
            await AssertNoContextCapture(fileWriter.WritePipelineFiles, release);
        }
        finally
        {
            foreach (var scope in fileWriter.GetScopes())
            {
                scope.Dispose();
            }
        }
    }

    private static IPipelineContextProvider CreateContextProvider()
    {
        var provider = new Mock<IPipelineContextProvider>();
        provider.Setup(x => x.GetModuleContext()).Returns(Mock.Of<IPipelineContext>());
        return provider.Object;
    }

    private static async Task AssertNoContextCapture(Func<Task> action, TaskCompletionSource release)
    {
        var context = new RecordingSynchronizationContext();
        var previousContext = SynchronizationContext.Current;
        Task operation;
        try
        {
            SynchronizationContext.SetSynchronizationContext(context);
            operation = action();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(previousContext);
            release.TrySetResult();
        }

        await operation.WaitAsync(TimeSpan.FromSeconds(10));
        await Assert.That(context.PostCount).IsEqualTo(0);
    }

    private sealed class RecordingSynchronizationContext : SynchronizationContext
    {
        private int _postCount;

        public int PostCount => Volatile.Read(ref _postCount);

        public override void Post(SendOrPostCallback callback, object? state)
        {
            Interlocked.Increment(ref _postCount);
            ThreadPool.QueueUserWorkItem(_ => callback(state));
        }
    }
}
