using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class RegistrationEventExecutorTests
{
    [Test]
    public async Task StartupCancellationReachesRegistrationHandlers()
    {
        using var cancellation = new CancellationTokenSource();
        using var engineCancellation = new ModularPipelines.Engine.EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>());
        var started = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var attributeEventService = new Mock<IModuleAttributeEventService>();
        attributeEventService.Setup(service => service.GetRegistrationHandlers(It.IsAny<Type>()))
            .Returns([Mock.Of<IModuleRegistrationHandler>()]);
        var invoker = new Mock<IEventHandlerInvoker>();
        invoker.Setup(service => service.InvokeRegistrationHandlersAsync(
                It.IsAny<IEnumerable<IModuleRegistrationHandler>>(),
                It.IsAny<IModuleRegistrationContext>(),
                It.IsAny<CancellationToken>()))
            .Returns(async (IEnumerable<IModuleRegistrationHandler> _, IModuleRegistrationContext _, CancellationToken token) =>
            {
                started.SetResult();
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
            });
        var executor = new RegistrationEventExecutor(
            attributeEventService.Object,
            invoker.Object,
            new ModuleDependencyRegistry(),
            Mock.Of<IModuleMetadataRegistry>(),
            Mock.Of<IConfiguration>(),
            Mock.Of<IHostEnvironment>(),
            engineCancellation);

        var registration = executor.InvokeRegistrationEventsAsync([new FirstModule()], cancellation.Token);
        try
        {
            await started.Task.WaitAsync(TimeSpan.FromSeconds(30));
        }
        finally
        {
            cancellation.Cancel();
        }

        await Assert.ThrowsAsync<OperationCanceledException>(() => registration);
        await Assert.That(engineCancellation.IsCancellationRequested).IsFalse();
    }

    [Test]
    public async Task Later_Invocation_Cannot_Add_Module_Types()
    {
        var attributeEventService = new Mock<IModuleAttributeEventService>();
        attributeEventService
            .Setup(service => service.GetRegistrationHandlers(It.IsAny<Type>()))
            .Returns([]);
        var executor = new RegistrationEventExecutor(
            attributeEventService.Object,
            Mock.Of<IEventHandlerInvoker>(),
            new ModuleDependencyRegistry(),
            Mock.Of<IModuleMetadataRegistry>(),
            Mock.Of<IConfiguration>(),
            Mock.Of<IHostEnvironment>(),
            new ModularPipelines.Engine.EngineCancellationToken(Mock.Of<IPrimaryExceptionContainer>()));

        await executor.InvokeRegistrationEventsAsync([new FirstModule()]);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            executor.InvokeRegistrationEventsAsync([new FirstModule(), new LaterModule()]));

        await Assert.That(exception!.Message).Contains(nameof(LaterModule));
    }

    private sealed class FirstModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class LaterModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }
}
