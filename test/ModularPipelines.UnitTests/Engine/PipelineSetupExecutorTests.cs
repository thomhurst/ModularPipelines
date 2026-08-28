using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel(nameof(PipelineSetupExecutorTests))]
public class PipelineSetupExecutorTests
{
    private sealed class OrderedPipelineHandler(int priority, string name, ICollection<string> calls)
        : IPipelineEventHandler
    {
        public int Priority => priority;

        public Task OnPipelineStartAsync(IPipelineContext context)
        {
            calls.Add(name);
            return Task.CompletedTask;
        }
    }

    private sealed class OrderedModuleHandler(int priority, string name, ICollection<string> calls)
        : IModuleEventHandler
    {
        public int Priority => priority;

        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            calls.Add(name);
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class CountingAttribute : Attribute
    {
        public CountingAttribute()
        {
            InstanceCount++;
        }

        public static int InstanceCount { get; set; }
    }

    [Counting]
    private sealed class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>(nameof(TestModule));
    }

    [Test]
    public async Task OnModuleReadyAsync_WithoutHooks_DoesNotCreateAttributes()
    {
        var executor = new PipelineSetupExecutor(
            [],
            [],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleAttributeEventService());
        var module = new TestModule();
        CountingAttribute.InstanceCount = 0;

        await executor.OnModuleReadyAsync(new ModuleState(module, module.GetType()));

        await Assert.That(CountingAttribute.InstanceCount).IsEqualTo(0);
    }

    [Test]
    public async Task OnModuleReadyAsync_WithHook_UsesCachedAttributeInstances()
    {
        var attributeEventService = new ModuleAttributeEventService();
        IReadOnlyList<Attribute>? handlerAttributes = null;
        var handler = new Mock<IModuleEventHandler>();
        handler.Setup(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>()))
            .Callback<IModuleHookContext>(context => handlerAttributes = context.ModuleAttributes)
            .Returns(Task.CompletedTask);
        var executor = new PipelineSetupExecutor(
            [],
            [handler.Object],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            attributeEventService);
        var module = new TestModule();

        await executor.OnModuleReadyAsync(new ModuleState(module, module.GetType()));

        await Assert.That(ReferenceEquals(
                handlerAttributes,
                attributeEventService.GetAttributes(module.GetType())))
            .IsTrue();
    }

    [Test]
    public async Task EventHandlers_Run_In_Priority_Order()
    {
        var calls = new List<string>();
        var pipelineContext = Mock.Of<IPipelineContext>();
        var contextProvider = new Mock<IPipelineContextProvider>();
        contextProvider.Setup(x => x.GetModuleContext()).Returns(pipelineContext);
        var executor = new PipelineSetupExecutor(
            [
                new OrderedPipelineHandler(20, "pipeline-20", calls),
                new OrderedPipelineHandler(10, "pipeline-10", calls),
            ],
            [
                new OrderedModuleHandler(20, "module-20", calls),
                new OrderedModuleHandler(10, "module-10", calls),
            ],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            contextProvider.Object,
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleAttributeEventService());
        var module = new TestModule();

        await executor.OnPipelineStartAsync();
        await executor.OnModuleReadyAsync(new ModuleState(module, module.GetType()));

        var expected = new[]
        {
            "pipeline-10",
            "pipeline-20",
            "module-10",
            "module-20",
        };

        await Assert.That(calls).Count().IsEqualTo(expected.Length);
        for (var index = 0; index < expected.Length; index++)
        {
            await Assert.That(calls[index]).IsEqualTo(expected[index]);
        }
    }

    [Test]
    public async Task Completion_Arguments_Are_Forwarded_To_Module_Event_Handlers()
    {
        var handler = new Mock<IModuleEventHandler>();
        handler.Setup(x => x.OnModuleEndAsync(It.IsAny<IModuleHookContext>(), It.IsAny<IModuleResult>()))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleFailureAsync(It.IsAny<IModuleHookContext>(), It.IsAny<Exception>()))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleSkippedAsync(It.IsAny<IModuleHookContext>(), It.IsAny<SkipDecision>()))
            .Returns(Task.CompletedTask);
        var executor = new PipelineSetupExecutor(
            [],
            [handler.Object],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleAttributeEventService());
        var module = new TestModule();
        var moduleState = new ModuleState(module, module.GetType());
        var result = Mock.Of<IModuleResult>();
        var exception = new InvalidOperationException("Expected failure");
        var skipDecision = SkipDecision.Skip("Expected skip");

        await executor.OnModuleEndAsync(moduleState, result);
        await executor.OnModuleFailureAsync(moduleState, exception);
        await executor.OnModuleSkippedAsync(moduleState, skipDecision);

        handler.Verify(x => x.OnModuleEndAsync(It.IsAny<IModuleHookContext>(), result), Times.Once);
        handler.Verify(x => x.OnModuleFailureAsync(It.IsAny<IModuleHookContext>(), exception), Times.Once);
        handler.Verify(x => x.OnModuleSkippedAsync(It.IsAny<IModuleHookContext>(), skipDecision), Times.Once);
    }
}
