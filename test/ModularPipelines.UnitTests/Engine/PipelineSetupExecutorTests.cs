using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Events;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel(nameof(PipelineSetupExecutorTests))]
public class PipelineSetupExecutorTests
{
    private sealed class OrderedPipelineHandler(int priority, string name, ICollection<string> calls)
        : IPipelineEventHandler
    {
        public int Order => priority;

        public Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            calls.Add(name);
            return Task.CompletedTask;
        }
    }

    private sealed class OrderedModuleHandler(int priority, string name, ICollection<string> calls)
        : IModuleEventHandler
    {
        public int Order => priority;

        public Task OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken)
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

        await executor.OnModuleReadyAsync(
            new ModuleState(module, module.GetType()),
            Mock.Of<IConsoleWriter>(), CancellationToken.None);

        await Assert.That(CountingAttribute.InstanceCount).IsEqualTo(0);
    }

    [Test]
    public async Task OnModuleReadyAsync_WithHook_UsesCachedAttributeInstances()
    {
        var attributeEventService = new ModuleAttributeEventService();
        IReadOnlyList<Attribute>? handlerAttributes = null;
        var handler = new Mock<IModuleEventHandler>();
        handler.Setup(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, CancellationToken>((context, _) => handlerAttributes = context.ModuleAttributes)
            .Returns(Task.CompletedTask);
        var executor = new PipelineSetupExecutor(
            [],
            [handler.Object],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            attributeEventService);
        var module = new TestModule();

        await executor.OnModuleReadyAsync(
            new ModuleState(module, module.GetType()),
            Mock.Of<IConsoleWriter>(), CancellationToken.None);

        await Assert.That(ReferenceEquals(
                handlerAttributes,
                attributeEventService.GetAttributes(module.GetType())))
            .IsTrue();
    }

    [Test]
    public async Task EventHandlers_Run_In_Ascending_Order()
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

        await executor.OnPipelineStartAsync(CancellationToken.None);
        await executor.OnModuleReadyAsync(
            new ModuleState(module, module.GetType()),
            Mock.Of<IConsoleWriter>(), CancellationToken.None);

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
        handler.Setup(x => x.OnModuleEndAsync(It.IsAny<IModuleHookContext>(), It.IsAny<IModuleResult>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleFailureAsync(It.IsAny<IModuleHookContext>(), It.IsAny<Exception>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleSkippedAsync(It.IsAny<IModuleHookContext>(), It.IsAny<SkipDecision>(), It.IsAny<CancellationToken>()))
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
        var writer = Mock.Of<IConsoleWriter>();

        await executor.OnModuleEndAsync(moduleState, result, writer, CancellationToken.None);
        await executor.OnModuleFailureAsync(moduleState, exception, writer, CancellationToken.None);
        await executor.OnModuleSkippedAsync(moduleState, result, skipDecision, writer, CancellationToken.None);

        handler.Verify(x => x.OnModuleEndAsync(It.IsAny<IModuleHookContext>(), result, It.IsAny<CancellationToken>()), Times.Once);
        handler.Verify(x => x.OnModuleFailureAsync(It.IsAny<IModuleHookContext>(), exception, It.IsAny<CancellationToken>()), Times.Once);
        handler.Verify(x => x.OnModuleSkippedAsync(It.IsAny<IModuleHookContext>(), skipDecision, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task ModuleEvents_UseProvidedConsoleWriter()
    {
        var receivedWriters = new List<IConsoleWriter>();
        var handler = new Mock<IModuleEventHandler>();
        handler.Setup(x => x.OnModuleReadyAsync(It.IsAny<IModuleHookContext>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, CancellationToken>((context, _) => receivedWriters.Add(context.Console))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleStartAsync(It.IsAny<IModuleHookContext>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, CancellationToken>((context, _) => receivedWriters.Add(context.Console))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleEndAsync(It.IsAny<IModuleHookContext>(), It.IsAny<IModuleResult>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, IModuleResult, CancellationToken>((context, _, _) => receivedWriters.Add(context.Console))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleFailureAsync(It.IsAny<IModuleHookContext>(), It.IsAny<Exception>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, Exception, CancellationToken>((context, _, _) => receivedWriters.Add(context.Console))
            .Returns(Task.CompletedTask);
        handler.Setup(x => x.OnModuleSkippedAsync(It.IsAny<IModuleHookContext>(), It.IsAny<SkipDecision>(), It.IsAny<CancellationToken>()))
            .Callback<IModuleHookContext, SkipDecision, CancellationToken>((context, _, _) => receivedWriters.Add(context.Console))
            .Returns(Task.CompletedTask);
        var executor = new PipelineSetupExecutor(
            [],
            [handler.Object],
            new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>()),
            Mock.Of<IPipelineContextProvider>(),
            Mock.Of<IModuleMetadataRegistry>(),
            new ModuleAttributeEventService());
        var module = new TestModule();
        var state = new ModuleState(module, module.GetType());
        var result = Mock.Of<IModuleResult>();
        var exception = new InvalidOperationException("Expected failure");
        var skipDecision = SkipDecision.Skip("Expected skip");
        var writer = Mock.Of<IConsoleWriter>();

        await executor.OnModuleReadyAsync(state, writer, CancellationToken.None);
        await executor.OnModuleStartAsync(state, writer, CancellationToken.None);
        await executor.OnModuleEndAsync(state, result, writer, CancellationToken.None);
        await executor.OnModuleFailureAsync(state, exception, writer, CancellationToken.None);
        await executor.OnModuleSkippedAsync(state, result, skipDecision, writer, CancellationToken.None);

        await Assert.That(receivedWriters.Count).IsEqualTo(5);
        foreach (var receivedWriter in receivedWriters)
        {
            await Assert.That(receivedWriter).IsSameReferenceAs(writer);
        }
    }
}
