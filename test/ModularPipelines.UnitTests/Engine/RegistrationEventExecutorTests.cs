using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class RegistrationEventExecutorTests
{
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
            Mock.Of<IHostEnvironment>());

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
