using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;
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
                await TestPipelineHostBuilder.Create()
                    .ConfigureServices((_, services) => services.AddSingleton(mediator.Object))
                    .AddModule<FailingModule>()
                    .ExecutePipelineAsync())
            .Throws<ModuleFailedException>();

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
}
