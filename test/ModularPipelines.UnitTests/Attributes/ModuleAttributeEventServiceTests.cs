using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleAttributeEventServiceTests
{
    public class TestStartAttribute : Attribute, IModuleStartEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context) => Task.CompletedTask;
    }

    public class TestFailureAttribute : Attribute, IModuleFailureEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleFailureAsync(IModuleEventContext context, Exception exception) => Task.CompletedTask;
    }

    [TestStart]
    [TestFailure]
    private class ModuleWithAttributes : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    private class ModuleWithoutAttributes : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    [Test]
    public async Task GetStartReceivers_ModuleWithAttribute_ReturnsReceiver()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithAttributes));

        await Assert.That(receivers.Count).IsEqualTo(1);
        await Assert.That(receivers[0]).IsTypeOf<TestStartAttribute>();
    }

    [Test]
    public async Task GetFailureReceivers_ModuleWithAttribute_ReturnsReceiver()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetFailureReceivers(typeof(ModuleWithAttributes));

        await Assert.That(receivers.Count).IsEqualTo(1);
        await Assert.That(receivers[0]).IsTypeOf<TestFailureAttribute>();
    }

    [Test]
    public async Task GetStartReceivers_ModuleWithoutAttributes_ReturnsEmpty()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithoutAttributes));

        await Assert.That(receivers).IsEmpty();
    }

    [Test]
    public async Task GetReceivers_CachesResults()
    {
        var service = new ModuleAttributeEventService();

        var receivers1 = service.GetStartReceivers(typeof(ModuleWithAttributes));
        var receivers2 = service.GetStartReceivers(typeof(ModuleWithAttributes));

        await Assert.That(ReferenceEquals(receivers1, receivers2)).IsTrue();
    }
}
