using Disposer = ModularPipelines.Helpers.Disposer;

namespace ModularPipelines.UnitTests.Engine;

public class DisposerTests
{
    private class MyClass : IAsyncDisposable
    {
        public bool DisposedAsync { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposedAsync = true;
            return ValueTask.CompletedTask;
        }
    }

    private class MyClass2 : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    [Test]
    public async Task Disposer_Calls_Async()
    {
        var myClass = new MyClass();
        await Assert.That(myClass.DisposedAsync).IsFalse();

        await Disposer.DisposeObjectAsync(myClass);
        await Assert.That(myClass.DisposedAsync).IsTrue();
    }

    [Test]
    public async Task Disposer_Calls_Sync()
    {
        var myClass = new MyClass2();
        await Assert.That(myClass.Disposed).IsFalse();

        await Disposer.DisposeObjectAsync(myClass);
        await Assert.That(myClass.Disposed).IsTrue();
    }

    [Test]
    public async Task Shutdown_Registration_Unsubscribes_The_Registered_Handler()
    {
        EventHandler? subscribedHandler = null;
        EventHandler? unsubscribedHandler = null;
        var registration = Disposer.RegisterOnShutdownWithUnregistration(
            new MyClass(),
            handler => subscribedHandler = handler,
            handler => unsubscribedHandler = handler);

        registration.Dispose();
        registration.Dispose();

        await Assert.That(subscribedHandler).IsNotNull();
        await Assert.That(unsubscribedHandler).IsSameReferenceAs(subscribedHandler);
    }
}
