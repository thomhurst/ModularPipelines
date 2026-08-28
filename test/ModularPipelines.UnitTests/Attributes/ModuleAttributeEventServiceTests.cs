using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Events;
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

    private sealed class CountingAttribute : Attribute
    {
        private static int _instanceCount;

        public CountingAttribute()
        {
            Interlocked.Increment(ref _instanceCount);
        }

        public static int InstanceCount => Volatile.Read(ref _instanceCount);

        public static void Reset() => Volatile.Write(ref _instanceCount, 0);
    }

    /// <summary>
    /// A start handler with priority 100 (runs last).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LowPriorityStartAttribute : Attribute, IModuleStartHandler
    {
        public bool ContinueOnError => false;
        public int Priority => 100;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start handler with priority 10 (runs second).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class MediumPriorityStartAttribute : Attribute, IModuleStartHandler
    {
        public bool ContinueOnError => false;
        public int Priority => 10;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    /// <summary>
    /// A start handler with priority 1 (runs first with explicit priority).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class HighPriorityStartAttribute : Attribute, IModuleStartHandler
    {
        public bool ContinueOnError => false;
        public int Priority => 1;

        public Task OnModuleStartAsync(IModuleHookContext context) => Task.CompletedTask;
    }

    [TestStart]
    [TestFailure]
    private class ModuleWithAttributes : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    private class ModuleWithoutAttributes : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [Counting]
    private class ModuleWithCountingAttribute : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    // Attributes are applied in reverse order of priority to test that sorting works
    [LowPriorityStart]   // Priority 100 - should be last
    [MediumPriorityStart] // Priority 10 - should be second
    [HighPriorityStart]  // Priority 1 - should be first
    private class ModuleWithPrioritizedHandlers : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    // Mix of prioritized and non-prioritized handlers
    [LowPriorityStart]   // Priority 100 - should be last
    [TestStart]          // No priority (defaults to 0) - should be first
    [HighPriorityStart]  // Priority 1 - should be second
    private class ModuleWithMixedPriorityHandlers : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
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
    public async Task GetAttributes_CachesResultsAndHandlerInstances()
    {
        var service = new ModuleAttributeEventService();

        var attributes1 = service.GetAttributes(typeof(ModuleWithAttributes));
        var attributes2 = service.GetAttributes(typeof(ModuleWithAttributes));
        var startHandler = service.GetStartHandlers(typeof(ModuleWithAttributes)).Single();

        await Assert.That(ReferenceEquals(attributes1, attributes2)).IsTrue();
        await Assert.That(ReferenceEquals(
                attributes1.OfType<TestStartAttribute>().Single(),
                startHandler))
            .IsTrue();
    }

    [Test]
    public async Task GetAttributes_ConcurrentCalls_CreateAttributesOnce()
    {
        var service = new ModuleAttributeEventService();
        CountingAttribute.Reset();

        var calls = Enumerable.Range(0, 32)
            .Select(_ => Task.Run(() => service.GetAttributes(typeof(ModuleWithCountingAttribute))))
            .ToArray();
        var results = await Task.WhenAll(calls);

        await Assert.That(CountingAttribute.InstanceCount).IsEqualTo(1);
        await Assert.That(results.All(attributes => ReferenceEquals(attributes, results[0]))).IsTrue();
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
