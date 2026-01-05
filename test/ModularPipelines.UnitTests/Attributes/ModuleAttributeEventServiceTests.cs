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

    /// <summary>
    /// A start receiver with priority 100 (runs last).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LowPriorityStartAttribute : Attribute, IModuleStartEventReceiver, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 100;

        public Task OnModuleStartAsync(IModuleEventContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start receiver with priority 10 (runs second).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MediumPriorityStartAttribute : Attribute, IModuleStartEventReceiver, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 10;

        public Task OnModuleStartAsync(IModuleEventContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start receiver with priority 1 (runs first with explicit priority).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HighPriorityStartAttribute : Attribute, IModuleStartEventReceiver, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 1;

        public Task OnModuleStartAsync(IModuleEventContext context) => Task.CompletedTask;
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

    // Attributes are applied in reverse order of priority to test that sorting works
    [LowPriorityStart]   // Priority 100 - should be last
    [MediumPriorityStart] // Priority 10 - should be second
    [HighPriorityStart]  // Priority 1 - should be first
    private class ModuleWithPrioritizedReceivers : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    // Mix of prioritized and non-prioritized receivers
    [LowPriorityStart]   // Priority 100 - should be last
    [TestStart]          // No priority (defaults to 0) - should be first
    [HighPriorityStart]  // Priority 1 - should be second
    private class ModuleWithMixedPriorityReceivers : Module<string>
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

    [Test]
    public async Task GetStartReceivers_WithPriority_ReturnsSortedByPriority()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithPrioritizedReceivers));

        await Assert.That(receivers.Count).IsEqualTo(3);
        // Lower priority values run first
        await Assert.That(receivers[0]).IsTypeOf<HighPriorityStartAttribute>();   // Priority 1
        await Assert.That(receivers[1]).IsTypeOf<MediumPriorityStartAttribute>(); // Priority 10
        await Assert.That(receivers[2]).IsTypeOf<LowPriorityStartAttribute>();    // Priority 100
    }

    [Test]
    public async Task GetStartReceivers_WithMixedPriority_DefaultsToZero()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithMixedPriorityReceivers));

        await Assert.That(receivers.Count).IsEqualTo(3);
        // Non-prioritized receiver (defaults to 0) should be first
        await Assert.That(receivers[0]).IsTypeOf<TestStartAttribute>();          // No priority (0)
        await Assert.That(receivers[1]).IsTypeOf<HighPriorityStartAttribute>();  // Priority 1
        await Assert.That(receivers[2]).IsTypeOf<LowPriorityStartAttribute>();   // Priority 100
    }

    [Test]
    public async Task GetStartReceivers_SingleReceiver_ReturnsWithoutSorting()
    {
        var service = new ModuleAttributeEventService();

        var receivers = service.GetStartReceivers(typeof(ModuleWithAttributes));

        // Single receiver should be returned as-is
        await Assert.That(receivers.Count).IsEqualTo(1);
        await Assert.That(receivers[0]).IsTypeOf<TestStartAttribute>();
    }
}
