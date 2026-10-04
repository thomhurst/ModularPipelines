using ModularPipelines.Events;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Interfaces;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class FailedModuleNotificationTests
{
    [Test]
    public async Task Failed_Module_Publishes_Unsuccessful_Completion_Notification()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Publish(
                It.IsAny<ModuleCompletedNotification>(),
                It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        await Assert.That(async () =>
                await TestPipelineBuilder.Create()
                    .ConfigureServices(services => services.AddKeyedSingleton<IMediator>(typeof(global::Mediator.Mediator), mediator.Object))
                    .AddModule<FailingModule>()
                    .RunAsync())
            .Throws<ModuleFailedException>();

        mediator.Verify(x => x.Publish(
            It.Is<ModuleCompletedNotification>(notification =>
                notification.ModuleState.ModuleType == typeof(FailingModule)
                && !notification.IsSuccessful),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Failed_Module_Publishes_Completion_When_Failure_Handler_Throws()
    {
        var mediator = new Mock<IMediator>();
        mediator
            .Setup(x => x.Publish(
                It.IsAny<ModuleCompletedNotification>(),
                It.IsAny<CancellationToken>()))
            .Returns(ValueTask.CompletedTask);

        // The module failure is preserved; the handler failure is surfaced alongside it.
        var exception = await Assert.ThrowsAsync<AggregateException>(async () =>
            await TestPipelineBuilder.Create()
                .ConfigureServices(services =>
                {
                    services.AddKeyedSingleton<IMediator>(typeof(global::Mediator.Mediator), mediator.Object);
                    services.AddSingleton<IModuleEventHandler, ThrowingFailureHandler>();
                })
                .AddModule<FailingModule>()
                .RunAsync());
        var inner = exception!.Flatten().InnerExceptions;
        await Assert.That(inner.OfType<ModuleFailedException>().Any()).IsTrue();
        await Assert.That(inner.Any(e => e.Message == "Expected handler failure")).IsTrue();

        mediator.Verify(x => x.Publish(
            It.Is<ModuleCompletedNotification>(notification =>
                notification.ModuleState.ModuleType == typeof(FailingModule)
                && !notification.IsSuccessful),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    private sealed class FailingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromException<bool>(new InvalidOperationException("Expected test failure"));
        }
    }

    private sealed class ThrowingFailureHandler : IModuleEventHandler
    {
        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception, CancellationToken cancellationToken)
        {
            return Task.FromException(new InvalidOperationException("Expected handler failure"));
        }
    }
}
