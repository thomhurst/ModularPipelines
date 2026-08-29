using ModularPipelines.Events;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class EventHandlerInvokerTests
{
    private class SuccessfulHandler : IModuleStartHandler
    {
        public bool WasCalled { get; private set; }

        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    private class FailingHandler : IModuleStartHandler
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    private class FailingHandlerWithContinue : IModuleStartHandler
    {
        public bool ContinueOnError => true;

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    [Test]
    public async Task InvokeAsync_CallsAllHandlers()
    {
        var handler1 = new SuccessfulHandler();
        var handler2 = new SuccessfulHandler();
        var handlers = new List<IModuleStartHandler> { handler1, handler2 };
        var invoker = new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>());
        var context = Mock.Of<IModuleHookContext>();

        await invoker.InvokeStartHandlersAsync(handlers, context);

        await Assert.That(handler1.WasCalled).IsTrue();
        await Assert.That(handler2.WasCalled).IsTrue();
    }

    [Test]
    public async Task InvokeAsync_HandlerThrows_ContinueOnErrorFalse_Propagates()
    {
        var handler = new FailingHandler();
        var handlers = new List<IModuleStartHandler> { handler };
        var logger = new Mock<ILogger<EventHandlerInvoker>>();
        var invoker = new EventHandlerInvoker(logger.Object);
        var context = Mock.Of<IModuleHookContext>();

        await Assert.That(async () => await invoker.InvokeStartHandlersAsync(handlers, context))
            .ThrowsException()
            .WithMessage("Test exception");
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains("Start handler FailingHandler failed", StringComparison.Ordinal)),
            It.IsAny<InvalidOperationException>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    public async Task InvokeAsync_HandlerThrows_ContinueOnErrorFalse_StillCallsRemainingHandlers()
    {
        var successHandler = new SuccessfulHandler();
        var handlers = new List<IModuleStartHandler> { new FailingHandler(), successHandler };
        var invoker = new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>());
        var context = Mock.Of<IModuleHookContext>();

        await Assert.That(async () => await invoker.InvokeStartHandlersAsync(handlers, context))
            .ThrowsException()
            .WithMessage("Test exception");
        await Assert.That(successHandler.WasCalled).IsTrue();
    }

    [Test]
    public async Task InvokeAsync_MultipleHandlersThrow_AggregatesFailures()
    {
        var handlers = new List<IModuleStartHandler> { new FailingHandler(), new FailingHandler() };
        var invoker = new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>());
        var context = Mock.Of<IModuleHookContext>();

        var exception = await Assert.That(async () => await invoker.InvokeStartHandlersAsync(handlers, context))
            .Throws<AggregateException>();

        await Assert.That(exception!.InnerExceptions).Count().IsEqualTo(2);
    }

    [Test]
    public async Task InvokeAsync_HandlerThrows_ContinueOnErrorTrue_Continues()
    {
        var failingHandler = new FailingHandlerWithContinue();
        var successHandler = new SuccessfulHandler();
        var handlers = new List<IModuleStartHandler> { failingHandler, successHandler };
        var invoker = new EventHandlerInvoker(Mock.Of<ILogger<EventHandlerInvoker>>());
        var context = Mock.Of<IModuleHookContext>();

        await invoker.InvokeStartHandlersAsync(handlers, context);

        await Assert.That(successHandler.WasCalled).IsTrue();
    }
}
