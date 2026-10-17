using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Distributed.Coordination;
using ModularPipelines.Distributed.SignalR.Coordination;
using ModularPipelines.Distributed.SignalR.Hub;
using ModularPipelines.Distributed.SignalR.Server;
using Moq;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class MasterServerHostTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Disposal_Waits_For_An_In_Flight_Completion_Notification(bool sendFails)
    {
        var sendStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var finishSend = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var client = new Mock<IClientProxy>();
        client.Setup(x => x.SendCoreAsync(HubMethodNames.MasterCompleted, It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(async () =>
            {
                sendStarted.SetResult();
                await finishSend.Task;
                if (sendFails)
                {
                    throw new InvalidOperationException("Completion send failed.");
                }
            });
        var clients = new Mock<IHubClients>();
        clients.SetupGet(x => x.All).Returns(client.Object);
        var hub = new Mock<IHubContext<DistributedPipelineHub>>();
        hub.SetupGet(x => x.Clients).Returns(clients.Object);
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddSingleton(hub.Object);
        await using var app = builder.Build();
        var coordinator = new InMemoryDistributedCoordinator(MsOptions.Create(new DistributedOptions()));
        var state = new SignalRMasterState(coordinator, "completion-test");
        await using var host = new MasterServerHost();
        // Supply a controlled SignalR transport so the send stays in flight until the test releases it.
        typeof(MasterServerHost).GetField("_app", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(host, app);
        typeof(MasterServerHost).GetField("_masterState", BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(host, state);

        var notification = host.NotifyCompletionAsync(default);
        await sendStarted.Task.WaitAsync(TimeSpan.FromSeconds(10));
        var disposal = host.DisposeAsync().AsTask();
        try
        {
            await Assert.That(disposal.IsCompleted).IsFalse();
            await Assert.That(app.Services.GetRequiredService<IHubContext<DistributedPipelineHub>>()).IsSameReferenceAs(hub.Object);
        }
        finally
        {
            finishSend.TrySetResult();
            if (sendFails)
            {
                await Assert.ThrowsAsync<InvalidOperationException>(() => notification.WaitAsync(TimeSpan.FromSeconds(10)));
            }
            else
            {
                await notification.WaitAsync(TimeSpan.FromSeconds(10));
            }
            await disposal.WaitAsync(TimeSpan.FromSeconds(10));
        }

        await host.NotifyCompletionAsync(default);
        client.Verify(x => x.SendCoreAsync(HubMethodNames.MasterCompleted, It.IsAny<object?[]>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
