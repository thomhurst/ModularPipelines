using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Helpers;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine.Execution;

public class ParallelLimitHandlerTests
{
    [Test]
    public async Task AcquireParallelLimitAsync_CancelsWhileWaiting()
    {
        var handler = CreateHandler(new PipelineOptions());
        using var heldSlot = await handler.AcquireParallelLimitAsync(
            typeof(ParallelLimitedModule),
            CancellationToken.None);
        using var cancellationTokenSource = new CancellationTokenSource();

        var waitingTask = handler.AcquireParallelLimitAsync(
            typeof(ParallelLimitedModule),
            cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => waitingTask);
    }

    [Test]
    public async Task AcquireExecutionTypeLimitAsync_CancelsWhileWaiting()
    {
        var handler = CreateHandler(new PipelineOptions
        {
            Concurrency = new ConcurrencyOptions
            {
                MaxCpuIntensiveModules = 1,
            },
        });
        var firstState = new ModuleState(new TestModule(), typeof(TestModule))
        {
            ExecutionType = ExecutionType.CpuIntensive,
        };
        var secondState = new ModuleState(new TestModule(), typeof(TestModule))
        {
            ExecutionType = ExecutionType.CpuIntensive,
        };
        using var heldSlot = await handler.AcquireExecutionTypeLimitAsync(
            firstState,
            CancellationToken.None);
        using var cancellationTokenSource = new CancellationTokenSource();

        var waitingTask = handler.AcquireExecutionTypeLimitAsync(
            secondState,
            cancellationTokenSource.Token);
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => waitingTask);
    }

    [Test]
    public async Task ModuleRunner_FiresReadyOnceBeforeLimitsAndMarksStartedAfter()
    {
        var limitWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLimitWait = new TaskCompletionSource<IDisposable>(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(typeof(TestModule), It.IsAny<CancellationToken>()))
            .Callback(() => limitWaitObserved.TrySetResult())
            .Returns(releaseLimitWait.Task);
        parallelLimitHandler
            .Setup(x => x.AcquireExecutionTypeLimitAsync(It.IsAny<ModuleState>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Mock.Of<IDisposable>());
        var receiver = new Mock<IModuleEventReceiver>();
        receiver
            .Setup(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()))
            .Returns(Task.CompletedTask);

        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        builder.Services.AddSingleton(receiver.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var scheduler = new Mock<IModuleScheduler>();
        scheduler
            .Setup(x => x.MarkModuleStarted(typeof(TestModule)))
            .Returns(false);
        var moduleState = new ModuleState(new TestModule(), typeof(TestModule));

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            CancellationToken.None);
        await limitWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));

        receiver.Verify(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()), Times.Once);
        scheduler.Verify(x => x.MarkModuleStarted(typeof(TestModule)), Times.Never);

        releaseLimitWait.TrySetResult(Mock.Of<IDisposable>());
        await executionTask;

        scheduler.Verify(x => x.MarkModuleStarted(typeof(TestModule)), Times.Once);

        await moduleRunner.ExecuteAsync(moduleState, scheduler.Object, CancellationToken.None);

        receiver.Verify(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()), Times.Once);
        scheduler.Verify(x => x.MarkModuleStarted(typeof(TestModule)), Times.Exactly(2));
    }

    [Test]
    public async Task ModuleRunner_PreservesAlwaysRunDuringCancelledLimiterWaits()
    {
        var observedTokens = new List<CancellationToken>();
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(AlwaysRunTestModule),
                It.IsAny<CancellationToken>()))
            .Callback<Type, CancellationToken>((_, token) => observedTokens.Add(token))
            .ReturnsAsync(Mock.Of<IDisposable>());
        parallelLimitHandler
            .Setup(x => x.AcquireExecutionTypeLimitAsync(
                It.IsAny<ModuleState>(),
                It.IsAny<CancellationToken>()))
            .Callback<ModuleState, CancellationToken>((_, token) => observedTokens.Add(token))
            .ReturnsAsync(Mock.Of<IDisposable>());

        var builder = TestPipelineBuilder.Create()
            .AddModule<AlwaysRunTestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var scheduler = new Mock<IModuleScheduler>();
        scheduler
            .Setup(x => x.MarkModuleStarted(typeof(AlwaysRunTestModule)))
            .Returns(false);
        var moduleState = new ModuleState(
            new AlwaysRunTestModule(),
            typeof(AlwaysRunTestModule));
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await moduleRunner.ExecuteWithoutDependencyWaitAsync(
            moduleState,
            scheduler.Object,
            cancellationTokenSource.Token);

        using (Assert.Multiple())
        {
            await Assert.That(observedTokens).Count().IsEqualTo(2);
            await Assert.That(observedTokens.All(token => !token.IsCancellationRequested)).IsTrue();
        }
    }

    private static ParallelLimitHandler CreateHandler(PipelineOptions options)
    {
        return new ParallelLimitHandler(
            new ParallelLimitProvider(Microsoft.Extensions.Options.Options.Create(options)),
            NullLogger<ParallelLimitHandler>.Instance);
    }

    private sealed record SingleSlotLimit : IParallelLimit
    {
        public static int Limit => 1;
    }

    [ModularPipelines.Attributes.ParallelLimiter<SingleSlotLimit>]
    private sealed class ParallelLimitedModule : TestModule;

    private class TestModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }

    private sealed class AlwaysRunTestModule : TestModule
    {
        protected override ModuleConfiguration Configure() =>
            ModuleConfiguration.Create()
                .WithAlwaysRun()
                .Build();
    }
}
