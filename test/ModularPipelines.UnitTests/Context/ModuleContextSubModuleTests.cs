using ModularPipelines.Reporting;
using ModularPipelines.Events;
using Mediator;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Context;

public class ModuleContextSubModuleTests
{
    [Test]
    public async Task RunSubModuleAsync_PublishesLifecycleAndSavesDuration()
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        var mediator = new Mock<IMediator>();
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        var expectedEstimate = TimeSpan.FromSeconds(12);
        SubModuleCreatedNotification? created = null;
        SubModuleCompletedNotification? completed = null;
        SubModuleEstimation? saved = null;

        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCreatedNotification>(), It.IsAny<CancellationToken>()))
            .Callback<SubModuleCreatedNotification, CancellationToken>((notification, _) => created = notification)
            .Returns(ValueTask.CompletedTask);
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()))
            .Callback<SubModuleCompletedNotification, CancellationToken>((notification, _) => completed = notification)
            .Returns(ValueTask.CompletedTask);
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new SubModuleEstimation("Compile", expectedEstimate)]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(module.GetType(), It.IsAny<SubModuleEstimation>(), It.IsAny<CancellationToken>()))
            .Callback<Type, SubModuleEstimation, CancellationToken>((_, estimation, _) => saved = estimation)
            .Returns(Task.CompletedTask);
        var context = CreateContext(module, executionContext, mediator.Object, estimatedTimeProvider.Object);

        var result = await context.RunSubModuleAsync("Compile", _ => Task.FromResult(42));

        await Assert.That(result).IsEqualTo(42);
        await Assert.That(created).IsNotNull();
        await Assert.That(created!.ParentModule).IsSameReferenceAs(module);
        await Assert.That(created.SubModule.Name).IsEqualTo("Compile");
        await Assert.That(created.EstimatedDuration).IsEqualTo(expectedEstimate);
        await Assert.That(completed).IsNotNull();
        await Assert.That(completed!.SubModule).IsSameReferenceAs(created.SubModule);
        await Assert.That(completed.IsSuccessful).IsTrue();
        await Assert.That(saved).IsNotNull();
        await Assert.That(saved!.SubModuleName).IsEqualTo("Compile");
        await Assert.That(saved.EstimatedDuration).IsEqualTo(created.SubModule.Duration);
    }

    [Test]
    public async Task RunSubModuleAsync_FailurePublishesCompletionWithoutSavingDuration()
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        var mediator = new Mock<IMediator>();
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        var expectedException = new InvalidOperationException("Submodule failed");
        SubModuleCreatedNotification? created = null;
        SubModuleCompletedNotification? completed = null;

        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCreatedNotification>(), It.IsAny<CancellationToken>()))
            .Callback<SubModuleCreatedNotification, CancellationToken>((notification, _) => created = notification)
            .Returns(ValueTask.CompletedTask);
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()))
            .Callback<SubModuleCompletedNotification, CancellationToken>((notification, _) => completed = notification)
            .Returns(ValueTask.CompletedTask);
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var context = CreateContext(module, executionContext, mediator.Object, estimatedTimeProvider.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => context.RunSubModuleAsync<int>("Fail", _ => Task.FromException<int>(expectedException)));

        await Assert.That(exception).IsSameReferenceAs(expectedException);
        await Assert.That(created).IsNotNull();
        await Assert.That(created!.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(2));
        await Assert.That(completed).IsNotNull();
        await Assert.That(completed!.IsSuccessful).IsFalse();
        estimatedTimeProvider.Verify(
            x => x.SaveSubModuleTimeAsync(It.IsAny<Type>(), It.IsAny<SubModuleEstimation>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    public async Task RunSubModuleAsync_LoadsEstimatesOncePerModuleExecution()
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        var mediator = new Mock<IMediator>();
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();

        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCreatedNotification>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(module.GetType(), It.IsAny<SubModuleEstimation>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var context = CreateContext(module, executionContext, mediator.Object, estimatedTimeProvider.Object);

        await Task.WhenAll(
            context.RunSubModuleAsync("First", _ => Task.CompletedTask),
            context.RunSubModuleAsync("Second", _ => Task.CompletedTask));

        estimatedTimeProvider.Verify(
            x => x.GetSubModuleEstimatedTimesAsync(module.GetType(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task RunSubModuleAsync_CanceledToken_DoesNotRunBody()
    {
        var module = Mock.Of<IModule>();
        var bodyWasRun = false;
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();
        var context = CreateContext(
            module,
            new ModuleExecutionContext(module, module.GetType()),
            Mock.Of<IMediator>(),
            Mock.Of<ISafeModuleEstimatedTimeProvider>());

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            context.RunSubModuleAsync(
                "Canceled",
                _ =>
                {
                    bodyWasRun = true;
                    return Task.CompletedTask;
                },
                cancellationTokenSource.Token));

        await Assert.That(bodyWasRun).IsFalse();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunSubModuleAsync_PassesCancellationTokenToBody(bool returnsResult)
    {
        var module = Mock.Of<IModule>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        using var moduleCancellation = executionContext.ModuleCancellationTokenSource;
        using var attemptCancellation = new CancellationTokenSource();
        executionContext.CurrentCancellationToken = attemptCancellation.Token;
        var context = CreateContext(executionContext);

        Task<int> Body(CancellationToken token)
        {
            receivedToken = token;
            return Task.FromResult(42);
        }

        if (returnsResult)
        {
            var result = await context.RunSubModuleAsync("Token-aware", Body, cancellationTokenSource.Token);
            await Assert.That(result).IsEqualTo(42);
        }
        else
        {
            await context.RunSubModuleAsync("Token-aware", token => (Task) Body(token), cancellationTokenSource.Token);
        }

        await Assert.That(receivedToken).IsEqualTo(cancellationTokenSource.Token);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunSubModuleAsync_DefaultToken_ObservesModuleCancellation(bool returnsResult)
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        using var moduleCancellation = executionContext.ModuleCancellationTokenSource;
        var context = CreateContext(executionContext);
        var receivedToken = CancellationToken.None;

        async Task<int> Body(CancellationToken token)
        {
            receivedToken = token;
            await moduleCancellation.CancelAsync();
            token.ThrowIfCancellationRequested();
            return 42;
        }

        var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => returnsResult
            ? context.RunSubModuleAsync("Canceled", Body)
            : context.RunSubModuleAsync("Canceled", token => (Task) Body(token)));

        await Assert.That(receivedToken).IsEqualTo(moduleCancellation.Token);
        await Assert.That(exception!.CancellationToken).IsEqualTo(moduleCancellation.Token);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunSubModuleAsync_DefaultToken_AlreadyCanceledModuleDoesNotStart(bool returnsResult)
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        using var moduleCancellation = executionContext.ModuleCancellationTokenSource;
        await moduleCancellation.CancelAsync();
        var mediator = new Mock<IMediator>();
        var context = CreateContext(executionContext, mediator.Object);
        var bodyWasRun = false;

        Task<int> Body(CancellationToken token)
        {
            bodyWasRun = true;
            return Task.FromResult(42);
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => returnsResult
            ? context.RunSubModuleAsync("Canceled", Body)
            : context.RunSubModuleAsync("Canceled", token => (Task) Body(token)));

        await Assert.That(bodyWasRun).IsFalse();
        mediator.Verify(
            x => x.Publish(It.IsAny<SubModuleCreatedNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
        mediator.Verify(
            x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task RunSubModuleAsync_CancellationDuringBody_CompletesWithoutMaskingOutcome(
        bool returnsResult, bool bodyFails)
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        using var moduleCancellation = executionContext.ModuleCancellationTokenSource;
        using var callerCancellation = new CancellationTokenSource();
        var expectedException = new InvalidOperationException("Submodule failed after cancellation");
        var mediator = new Mock<IMediator>();
        SubModuleCompletedNotification? completed = null;
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()))
            .Callback<SubModuleCompletedNotification, CancellationToken>((notification, token) =>
            {
                token.ThrowIfCancellationRequested();
                completed = notification;
            })
            .Returns(ValueTask.CompletedTask);
        var context = CreateContext(executionContext, mediator.Object);

        async Task<int> Body(CancellationToken token)
        {
            await callerCancellation.CancelAsync();
            if (bodyFails)
            {
                throw expectedException;
            }

            return 42;
        }

        Task Run() => returnsResult
            ? context.RunSubModuleAsync("Canceled", Body, callerCancellation.Token)
            : context.RunSubModuleAsync("Canceled", token => (Task) Body(token), callerCancellation.Token);

        if (bodyFails)
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(Run);
            await Assert.That(exception).IsSameReferenceAs(expectedException);
        }
        else
        {
            await Run();
        }

        await Assert.That(completed).IsNotNull();
        await Assert.That(completed!.IsSuccessful).IsEqualTo(!bodyFails);
        mediator.Verify(
            x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), CancellationToken.None),
            Times.Once);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RunSubModuleAsync_ConcurrentAttempts_KeepTheirTokensAndRestoreModuleFallback(bool returnsResult)
    {
        var module = Mock.Of<IModule>();
        var executionContext = new ModuleExecutionContext(module, module.GetType());
        using var moduleCancellation = executionContext.ModuleCancellationTokenSource;
        using var firstAttempt = new CancellationTokenSource();
        using var secondAttempt = new CancellationTokenSource();
        var context = CreateContext(executionContext);
        var resume = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        async Task<CancellationToken> RunAttempt(CancellationToken attemptToken)
        {
            executionContext.CurrentCancellationToken = attemptToken;
            await resume.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            var receivedToken = CancellationToken.None;

            Task<CancellationToken> Body(CancellationToken token)
            {
                receivedToken = token;
                return Task.FromResult(token);
            }

            if (returnsResult)
            {
                return await context.RunSubModuleAsync("Attempt", Body);
            }

            await context.RunSubModuleAsync("Attempt", token => (Task) Body(token));
            return receivedToken;
        }

        var first = RunAttempt(firstAttempt.Token);
        var second = RunAttempt(secondAttempt.Token);
        resume.SetResult();

        await Assert.That(await first).IsEqualTo(firstAttempt.Token);
        await Assert.That(await second).IsEqualTo(secondAttempt.Token);
        var fallback = await context.RunSubModuleAsync("Outside attempt", token => Task.FromResult(token));
        await Assert.That(fallback).IsEqualTo(moduleCancellation.Token);
    }

    private static ModuleContext CreateContext(ModuleExecutionContext executionContext, IMediator? mediator = null)
    {
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(executionContext.ModuleType, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(executionContext.ModuleType, It.IsAny<SubModuleEstimation>(), It.IsAny<CancellationToken>()))
            .Callback<Type, SubModuleEstimation, CancellationToken>((_, _, token) => token.ThrowIfCancellationRequested())
            .Returns(Task.CompletedTask);

        return CreateContext(
            executionContext.Module,
            executionContext,
            mediator ?? Mock.Of<IMediator>(),
            estimatedTimeProvider.Object);
    }

    private static ModuleContext CreateContext(
        IModule module,
        ModuleExecutionContext executionContext,
        IMediator mediator,
        ISafeModuleEstimatedTimeProvider estimatedTimeProvider)
    {
        var pipelineContext = new Mock<IPipelineContext>();
        pipelineContext.As<IInternalPipelineContext>();

        return new ModuleContext(
            pipelineContext.Object,
            module,
            executionContext,
            Mock.Of<IModuleLogger>(),
            mediator,
            estimatedTimeProvider);
    }
}
