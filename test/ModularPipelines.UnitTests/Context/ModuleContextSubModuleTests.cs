using Mediator;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Events;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
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
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType()))
            .ReturnsAsync([new SubModuleEstimation("Compile", expectedEstimate)]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(module.GetType(), It.IsAny<SubModuleEstimation>()))
            .Callback<Type, SubModuleEstimation>((_, estimation) => saved = estimation)
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
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType()))
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
            x => x.SaveSubModuleTimeAsync(It.IsAny<Type>(), It.IsAny<SubModuleEstimation>()),
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
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType()))
            .ReturnsAsync([]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(module.GetType(), It.IsAny<SubModuleEstimation>()))
            .Returns(Task.CompletedTask);
        var context = CreateContext(module, executionContext, mediator.Object, estimatedTimeProvider.Object);

        await Task.WhenAll(
            context.RunSubModuleAsync("First", _ => Task.CompletedTask),
            context.RunSubModuleAsync("Second", _ => Task.CompletedTask));

        estimatedTimeProvider.Verify(
            x => x.GetSubModuleEstimatedTimesAsync(module.GetType()),
            Times.Once);
    }

    [Test]
    public async Task RunSubModuleAsync_CancelledToken_DoesNotRunBody()
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
                "Cancelled",
                _ =>
                {
                    bodyWasRun = true;
                    return Task.CompletedTask;
                },
                cancellationTokenSource.Token));

        await Assert.That(bodyWasRun).IsFalse();
    }

    [Test]
    public async Task RunSubModuleAsync_PassesCancellationTokenToBody()
    {
        var module = Mock.Of<IModule>();
        using var cancellationTokenSource = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCreatedNotification>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
        mediator
            .Setup(x => x.Publish(It.IsAny<SubModuleCompletedNotification>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);
        var estimatedTimeProvider = new Mock<ISafeModuleEstimatedTimeProvider>();
        estimatedTimeProvider
            .Setup(x => x.GetSubModuleEstimatedTimesAsync(module.GetType()))
            .ReturnsAsync([]);
        estimatedTimeProvider
            .Setup(x => x.SaveSubModuleTimeAsync(module.GetType(), It.IsAny<SubModuleEstimation>()))
            .Returns(Task.CompletedTask);
        var context = CreateContext(
            module,
            new ModuleExecutionContext(module, module.GetType()),
            mediator.Object,
            estimatedTimeProvider.Object);

        await context.RunSubModuleAsync(
            "Token-aware",
            token =>
            {
                receivedToken = token;
                return Task.CompletedTask;
            },
            cancellationTokenSource.Token);

        await Assert.That(receivedToken).IsEqualTo(cancellationTokenSource.Token);
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
