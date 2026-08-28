using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Enums;
using ModularPipelines.Events;
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
    public async Task ModuleRunner_FiresGlobalAndAttributeReadyOnceBeforeLimitsAndMarksStartedAfter()
    {
        TrackingReadyAttribute.Reset();
        var limitWaitObserved = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLimitWait = new TaskCompletionSource<IDisposable>(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(typeof(ReadyTestModule), It.IsAny<CancellationToken>()))
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
            .AddModule<ReadyTestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        builder.Services.AddSingleton(receiver.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var scheduler = new Mock<IModuleScheduler>();
        scheduler
            .Setup(x => x.MarkModuleStarted(typeof(ReadyTestModule)))
            .Returns(false);
        var moduleState = new ModuleState(new ReadyTestModule(), typeof(ReadyTestModule));

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            CancellationToken.None);
        await limitWaitObserved.Task.WaitAsync(TimeSpan.FromSeconds(2));

        receiver.Verify(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()), Times.Once);
        await Assert.That(TrackingReadyAttribute.InvocationCount).IsEqualTo(1);
        scheduler.Verify(x => x.MarkModuleStarted(typeof(ReadyTestModule)), Times.Never);

        releaseLimitWait.TrySetResult(Mock.Of<IDisposable>());
        await executionTask;

        scheduler.Verify(x => x.MarkModuleStarted(typeof(ReadyTestModule)), Times.Once);

        await moduleRunner.ExecuteAsync(moduleState, scheduler.Object, CancellationToken.None);

        receiver.Verify(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()), Times.Once);
        await Assert.That(TrackingReadyAttribute.InvocationCount).IsEqualTo(1);
        scheduler.Verify(x => x.MarkModuleStarted(typeof(ReadyTestModule)), Times.Exactly(2));
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

    [Test]
    public async Task ModuleRunner_DoesNotReplaceResultRegisteredBeforeLimiterCancellation()
    {
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromException<IDisposable>(new OperationCanceledException()));

        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var resultRegistrar = host.Services.GetRequiredService<IModuleResultRegistrar>();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var scheduler = new Mock<IModuleScheduler>();
        var module = new TestModule();
        var moduleState = new ModuleState(module, typeof(TestModule));
        var originalException = new InvalidOperationException("Original pipeline failure");
        resultRegistrar.RegisterTerminatedResult(module, typeof(TestModule), originalException);

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            moduleRunner.ExecuteAsync(
                moduleState,
                scheduler.Object,
                CancellationToken.None));

        var registeredResult = resultRegistry.GetResult(typeof(TestModule));
        var awaitedResult = await module;
        using (Assert.Multiple())
        {
            await Assert.That(registeredResult).IsSameReferenceAs(awaitedResult);
            await Assert.That(awaitedResult.ExceptionOrDefault).IsSameReferenceAs(originalException);
        }
    }

    [Test]
    public async Task ModuleRunner_EngineCancellationStopsLimiterWaitAsPipelineTerminated()
    {
        var limiterWaitStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns<Type, CancellationToken>(async (_, token) =>
            {
                limiterWaitStarted.TrySetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return Mock.Of<IDisposable>();
            });

        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModule>();
        var logger = new Mock<ILogger<ModuleRunner>>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        builder.Services.AddSingleton(logger.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var engineCancellationToken = host.Services.GetRequiredService<ModularPipelines.Engine.EngineCancellationToken>();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var scheduler = new Mock<IModuleScheduler>();
        var moduleState = new ModuleState(new TestModule(), typeof(TestModule));
        using var workerCancellationTokenSource = new CancellationTokenSource();

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            workerCancellationTokenSource.Token);
        await limiterWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        engineCancellationToken.Cancel();

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => executionTask);
        await Assert.That(exception!.CancellationToken)
            .IsEqualTo(workerCancellationTokenSource.Token);
        scheduler.Verify(x => x.MarkModuleCompleted(
            typeof(TestModule),
            false,
            It.IsAny<OperationCanceledException>(),
            ModuleStatus.Cancelled), Times.Once);
        await Assert.That(resultRegistry.GetResult(typeof(TestModule))!.Status)
            .IsEqualTo(ModuleStatus.Cancelled);
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception?>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Never);
    }

    [Test]
    public async Task ModuleRunner_AlwaysRunEngineCancellationNormalizesLimiterWaitToWorkerToken()
    {
        var limiterWaitStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(AlwaysRunTestModule),
                It.IsAny<CancellationToken>()))
            .Returns<Type, CancellationToken>(async (_, token) =>
            {
                limiterWaitStarted.TrySetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return Mock.Of<IDisposable>();
            });

        var builder = TestPipelineBuilder.Create()
            .AddModule<AlwaysRunTestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var engineCancellationToken = host.Services.GetRequiredService<ModularPipelines.Engine.EngineCancellationToken>();
        var scheduler = new Mock<IModuleScheduler>();
        var moduleState = new ModuleState(
            new AlwaysRunTestModule(),
            typeof(AlwaysRunTestModule));
        using var workerCancellationTokenSource = new CancellationTokenSource();

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            workerCancellationTokenSource.Token);
        await limiterWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        engineCancellationToken.CancelWithReason("User cancellation");

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => executionTask);

        await Assert.That(exception!.CancellationToken)
            .IsEqualTo(workerCancellationTokenSource.Token);
    }

    [Test]
    public async Task ModuleRunner_PreservesWorkerTokenForFailureDrivenEngineCancellation()
    {
        var limiterWaitStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns<Type, CancellationToken>(async (_, token) =>
            {
                limiterWaitStarted.TrySetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return Mock.Of<IDisposable>();
            });

        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var engineCancellationToken = host.Services.GetRequiredService<ModularPipelines.Engine.EngineCancellationToken>();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var scheduler = new Mock<IModuleScheduler>();
        var module = new TestModule();
        var moduleState = new ModuleState(module, typeof(TestModule));
        using var workerCancellationTokenSource = new CancellationTokenSource();
        var originalException = new InvalidOperationException("Primary module failure");

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            workerCancellationTokenSource.Token);
        await limiterWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        engineCancellationToken.CancelWithException(originalException);

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => executionTask);
        var registeredResult = resultRegistry.GetResult(typeof(TestModule));
        var awaitedResult = await module;

        using (Assert.Multiple())
        {
            await Assert.That(exception!.CancellationToken)
                .IsEqualTo(workerCancellationTokenSource.Token);
            await Assert.That(registeredResult).IsSameReferenceAs(awaitedResult);
            await Assert.That(awaitedResult.ExceptionOrDefault).IsSameReferenceAs(originalException);
        }

        scheduler.Verify(x => x.MarkModuleCompleted(
            typeof(TestModule),
            false,
            originalException,
            ModuleStatus.Cancelled), Times.Once);
    }

    [Test]
    public async Task ModuleRunner_RoutesThrowingReadyHandlerThroughFailureLifecycle()
    {
        ThrowingReadyAttribute.Reset();
        var receiver = new TrackingFailureReceiver();
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Publish(
                It.IsAny<ModuleCompletedNotification>(),
                It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
        var builder = TestPipelineBuilder.Create()
            .AddModule<ThrowingReadyTestModule>();
        builder.Services.AddSingleton<IModuleEventReceiver>(receiver);
        builder.Services.AddSingleton(mediator.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var scheduler = new Mock<IModuleScheduler>();
        var moduleState = new ModuleState(
            new ThrowingReadyTestModule(),
            typeof(ThrowingReadyTestModule));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            moduleRunner.ExecuteAsync(moduleState, scheduler.Object, CancellationToken.None));

        using (Assert.Multiple())
        {
            await Assert.That(resultRegistry.GetResult(typeof(ThrowingReadyTestModule))!.Status)
                .IsEqualTo(ModuleStatus.Failed);
            await Assert.That(ThrowingReadyAttribute.FailureInvocationCount).IsEqualTo(1);
            await Assert.That(receiver.FailureInvocationCount).IsEqualTo(1);
        }

        mediator.Verify(x => x.Publish(
            It.Is<ModuleCompletedNotification>(notification =>
                notification.ModuleState.ModuleType == typeof(ThrowingReadyTestModule)
                && !notification.IsSuccessful),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ModuleRunner_PreservesWorkerTokenForCancelledLinkedLimiterWait()
    {
        var limiterWaitStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var parallelLimitHandler = new Mock<IParallelLimitHandler>();
        parallelLimitHandler
            .Setup(x => x.AcquireParallelLimitAsync(
                typeof(TestModule),
                It.IsAny<CancellationToken>()))
            .Returns<Type, CancellationToken>(async (_, token) =>
            {
                limiterWaitStarted.TrySetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
                return Mock.Of<IDisposable>();
            });

        var builder = TestPipelineBuilder.Create()
            .AddModule<TestModule>();
        builder.Services.AddSingleton(parallelLimitHandler.Object);
        await using var host = await builder.BuildAsync();
        var moduleRunner = host.Services.GetRequiredService<IModuleRunner>();
        var scheduler = new Mock<IModuleScheduler>();
        var moduleState = new ModuleState(new TestModule(), typeof(TestModule));
        using var workerCancellationTokenSource = new CancellationTokenSource();

        var executionTask = moduleRunner.ExecuteAsync(
            moduleState,
            scheduler.Object,
            workerCancellationTokenSource.Token);
        await limiterWaitStarted.Task.WaitAsync(TimeSpan.FromSeconds(2));
        workerCancellationTokenSource.Cancel();

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => executionTask);

        await Assert.That(exception!.CancellationToken)
            .IsEqualTo(workerCancellationTokenSource.Token);
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

    [TrackingReady]
    private sealed class ReadyTestModule : TestModule;

    [ThrowingReady]
    private sealed class ThrowingReadyTestModule : TestModule;

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class TrackingReadyAttribute : Attribute, IModuleReadyHandler
    {
        private static int _invocationCount;

        public static int InvocationCount => Volatile.Read(ref _invocationCount);

        public static void Reset() => Volatile.Write(ref _invocationCount, 0);

        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            Interlocked.Increment(ref _invocationCount);
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class ThrowingReadyAttribute : Attribute, IModuleReadyHandler, IModuleFailureHandler
    {
        private static int _failureInvocationCount;

        public static int FailureInvocationCount => Volatile.Read(ref _failureInvocationCount);

        public static void Reset() => Volatile.Write(ref _failureInvocationCount, 0);

        public Task OnModuleReadyAsync(IModuleHookContext context) =>
            Task.FromException(new InvalidOperationException("Ready handler failure"));

        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception)
        {
            Interlocked.Increment(ref _failureInvocationCount);
            return Task.CompletedTask;
        }
    }

    private sealed class TrackingFailureReceiver : IModuleEventReceiver
    {
        private int _failureInvocationCount;

        public int FailureInvocationCount => Volatile.Read(ref _failureInvocationCount);

        public Task OnModuleFailureAsync(IModuleHookContext context)
        {
            Interlocked.Increment(ref _failureInvocationCount);
            return Task.CompletedTask;
        }
    }

    private sealed class AlwaysRunTestModule : TestModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
                .WithAlwaysRun();
    }
}
