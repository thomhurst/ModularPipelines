using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleAttributeEventServiceTests
{
    public class TestStartAttribute : Attribute, IModuleStartHandler
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    public class TestFailureAttribute : Attribute, IModuleFailureHandler
    {
        public bool ContinueOnError => false;

        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception) => Task.CompletedTask;
    }

    /// <summary>
    /// A start handler with priority 100 (runs last).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LowPriorityStartAttribute : Attribute, IModuleStartHandler, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 100;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start handler with priority 10 (runs second).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MediumPriorityStartAttribute : Attribute, IModuleStartHandler, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 10;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start handler with priority 1 (runs first with explicit priority).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HighPriorityStartAttribute : Attribute, IModuleStartHandler, IEventHandlerPriority
    {
        public bool ContinueOnError => false;
        public int Priority => 1;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    [TestStart]
    [TestFailure]
    private class ModuleWithAttributes : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    private class ModuleWithoutAttributes : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    // Attributes are applied in reverse order of priority to test that sorting works
    [LowPriorityStart]   // Priority 100 - should be last
    [MediumPriorityStart] // Priority 10 - should be second
    [HighPriorityStart]  // Priority 1 - should be first
    private class ModuleWithPrioritizedHandlers : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    // Mix of prioritized and non-prioritized handlers
    [LowPriorityStart]   // Priority 100 - should be last
    [TestStart]          // No priority (defaults to 0) - should be first
    [HighPriorityStart]  // Priority 1 - should be second
    private class ModuleWithMixedPriorityHandlers : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("test");
    }

    [Test]
    public async Task GetStartHandlers_ModuleWithAttribute_ReturnsHandler()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ModuleWithAttributes));

        await Assert.That(handlers.Count).IsEqualTo(1);
        await Assert.That(handlers[0]).IsTypeOf<TestStartAttribute>();
    }

    [Test]
    public async Task GetFailureHandlers_ModuleWithAttribute_ReturnsHandler()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetFailureHandlers(typeof(ModuleWithAttributes));

        await Assert.That(handlers.Count).IsEqualTo(1);
        await Assert.That(handlers[0]).IsTypeOf<TestFailureAttribute>();
    }

    [Test]
    public async Task GetStartHandlers_ModuleWithoutAttributes_ReturnsEmpty()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ModuleWithoutAttributes));

        await Assert.That(handlers).IsEmpty();
    }

    [Test]
    public async Task GetHandlers_CachesResults()
    {
        var service = new ModuleAttributeEventService();

        var handlers1 = service.GetStartHandlers(typeof(ModuleWithAttributes));
        var handlers2 = service.GetStartHandlers(typeof(ModuleWithAttributes));

        await Assert.That(ReferenceEquals(handlers1, handlers2)).IsTrue();
    }

    [Test]
    public async Task GetStartHandlers_WithPriority_ReturnsSortedByPriority()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ModuleWithPrioritizedHandlers));

        await Assert.That(handlers.Count).IsEqualTo(3);
        // Lower priority values run first
        await Assert.That(handlers[0]).IsTypeOf<HighPriorityStartAttribute>();   // Priority 1
        await Assert.That(handlers[1]).IsTypeOf<MediumPriorityStartAttribute>(); // Priority 10
        await Assert.That(handlers[2]).IsTypeOf<LowPriorityStartAttribute>();    // Priority 100
    }

    [Test]
    public async Task GetStartHandlers_WithMixedPriority_DefaultsToZero()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ModuleWithMixedPriorityHandlers));

        await Assert.That(handlers.Count).IsEqualTo(3);
        // Non-prioritized handler (defaults to 0) should be first
        await Assert.That(handlers[0]).IsTypeOf<TestStartAttribute>();          // No priority (0)
        await Assert.That(handlers[1]).IsTypeOf<HighPriorityStartAttribute>();  // Priority 1
        await Assert.That(handlers[2]).IsTypeOf<LowPriorityStartAttribute>();   // Priority 100
    }

    [Test]
    public async Task GetStartHandlers_SingleHandler_ReturnsWithoutSorting()
    {
        var service = new ModuleAttributeEventService();

        var handlers = service.GetStartHandlers(typeof(ModuleWithAttributes));

        // Single handler should be returned as-is
        await Assert.That(handlers.Count).IsEqualTo(1);
        await Assert.That(handlers[0]).IsTypeOf<TestStartAttribute>();
    }
}
