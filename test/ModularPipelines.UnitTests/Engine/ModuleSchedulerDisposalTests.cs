using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Engine.Scheduling;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class ModuleSchedulerDisposalTests
{
    private sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(TestModule));
    }

    [Test]
    public async Task Dispose_WhileStateTransitionIsInFlight_DoesNotThrow()
    {
        var transitionEntered = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var continueTransition = new ManualResetEventSlim();
        var constraintEvaluator = new Mock<IModuleConstraintEvaluator>();
        constraintEvaluator
            .Setup(x => x.CanStartExecution(It.IsAny<ModuleState>(), It.IsAny<IEnumerable<ModuleState>>()))
            .Returns(() =>
            {
                transitionEntered.TrySetResult();
                continueTransition.Wait(TimeSpan.FromSeconds(5));
                return true;
            });
        var scheduler = CreateScheduler(constraintEvaluator.Object);
        scheduler.InitializeModules([new TestModule()]);

        var transitionTask = Task.Run(() => scheduler.MarkModuleStarted(typeof(TestModule)));
        await transitionEntered.Task.WaitAsync(TimeSpan.FromSeconds(5));

        Exception? disposeException = null;
        try
        {
            scheduler.Dispose();
        }
        catch (Exception exception)
        {
            disposeException = exception;
        }
        finally
        {
            continueTransition.Set();
        }

        Exception? transitionException = null;
        try
        {
            await transitionTask.WaitAsync(TimeSpan.FromSeconds(5));
        }
        catch (Exception exception)
        {
            transitionException = exception;
        }

        await Assert.That(disposeException).IsNull();
        await Assert.That(transitionException).IsNull();
    }

    [Test]
    public async Task MarkModuleCompleted_AfterDispose_DoesNotThrow()
    {
        var scheduler = CreateScheduler(Mock.Of<IModuleConstraintEvaluator>());
        scheduler.InitializeModules([new TestModule()]);
        scheduler.Dispose();

        await Assert.That(() => scheduler.MarkModuleCompleted(typeof(TestModule), success: false))
            .ThrowsNothing();
    }

    private static ModuleScheduler CreateScheduler(IModuleConstraintEvaluator constraintEvaluator)
    {
        return new ModuleScheduler(
            NullLogger.Instance,
            TimeProvider.System,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            new ModuleDependencyRegistry(),
            new ModuleMetadataRegistry(new ModuleAttributeEventService()),
            Mock.Of<IMetricsCollector>(),
            constraintEvaluator,
            Mock.Of<ISchedulerStatusReporter>());
    }
}
