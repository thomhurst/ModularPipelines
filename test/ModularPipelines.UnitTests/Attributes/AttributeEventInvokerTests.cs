using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Attributes;
using Moq;

namespace ModularPipelines.UnitTests.Attributes;

public class AttributeEventInvokerTests
{
    private class SuccessfulReceiver : IModuleStartEventReceiver
    {
        public bool WasCalled { get; private set; }

        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            WasCalled = true;
            return Task.CompletedTask;
        }
    }

    private class FailingReceiver : IModuleStartEventReceiver
    {
        public bool ContinueOnError => false;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    private class FailingReceiverWithContinue : IModuleStartEventReceiver
    {
        public bool ContinueOnError => true;

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            throw new InvalidOperationException("Test exception");
        }
    }

    [Test]
    public async Task InvokeAsync_CallsAllReceivers()
    {
        var receiver1 = new SuccessfulReceiver();
        var receiver2 = new SuccessfulReceiver();
        var receivers = new List<IModuleStartEventReceiver> { receiver1, receiver2 };
        var invoker = new AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>());
        var context = Mock.Of<IModuleEventContext>();

        await invoker.InvokeStartReceiversAsync(receivers, context);

        await Assert.That(receiver1.WasCalled).IsTrue();
        await Assert.That(receiver2.WasCalled).IsTrue();
    }

    [Test]
    public async Task InvokeAsync_ReceiverThrows_ContinueOnErrorFalse_Propagates()
    {
        var receiver = new FailingReceiver();
        var receivers = new List<IModuleStartEventReceiver> { receiver };
        var invoker = new AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>());
        var context = Mock.Of<IModuleEventContext>();

        await Assert.That(async () => await invoker.InvokeStartReceiversAsync(receivers, context))
            .ThrowsException()
            .WithMessage("Test exception");
    }

    [Test]
    public async Task InvokeAsync_ReceiverThrows_ContinueOnErrorTrue_Continues()
    {
        var failingReceiver = new FailingReceiverWithContinue();
        var successReceiver = new SuccessfulReceiver();
        var receivers = new List<IModuleStartEventReceiver> { failingReceiver, successReceiver };
        var invoker = new AttributeEventInvoker(Mock.Of<ILogger<AttributeEventInvoker>>());
        var context = Mock.Of<IModuleEventContext>();

        await invoker.InvokeStartReceiversAsync(receivers, context);

        await Assert.That(successReceiver.WasCalled).IsTrue();
    }
}
