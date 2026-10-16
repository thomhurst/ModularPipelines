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

    [Test]
    public async Task Shutdown_Waits_For_Asynchronous_Disposal()
    {
        var disposed = false;
        EventHandler? handler = null;
        using var registration = Disposer.RegisterOnShutdownWithUnregistration(
            async () =>
            {
                await Task.Delay(50);
                disposed = true;
            },
            subscribed => handler = subscribed,
            _ => { });

        handler!(null, EventArgs.Empty);

        await Assert.That(disposed).IsTrue();
    }

    [Test]
    public async Task Shutdown_Bounds_Synchronous_Work()
    {
        using var release = new ManualResetEventSlim();
        var finished = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        EventHandler? handler = null;
        using var registration = Disposer.RegisterOnShutdownWithUnregistration(
            () =>
            {
                release.Wait();
                finished.SetResult();
                return Task.CompletedTask;
            },
            subscribed => handler = subscribed,
            _ => { },
            TimeSpan.FromMilliseconds(50));

        try
        {
            await Task.Run(() => handler!(null, EventArgs.Empty)).WaitAsync(TimeSpan.FromSeconds(5));
            await Assert.That(finished.Task.IsCompleted).IsFalse();
        }
        finally
        {
            release.Set();
            await finished.Task.WaitAsync(TimeSpan.FromSeconds(5));
        }
    }

    [Test]
    public async Task Shutdown_Bounds_Asynchronous_Work()
    {
        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        EventHandler? handler = null;
        using var registration = Disposer.RegisterOnShutdownWithUnregistration(
            () => completion.Task,
            subscribed => handler = subscribed,
            _ => { },
            TimeSpan.FromMilliseconds(50));

        try
        {
            await Task.Run(() => handler!(null, EventArgs.Empty)).WaitAsync(TimeSpan.FromSeconds(5));
            await Assert.That(completion.Task.IsCompleted).IsFalse();
        }
        finally
        {
            completion.SetResult();
        }
    }

    [Test]
    public void Shutdown_Suppresses_Disposal_Failures()
    {
        EventHandler? handler = null;
        using var registration = Disposer.RegisterOnShutdownWithUnregistration(
            () => Task.FromException(new InvalidOperationException("shutdown failure")),
            subscribed => handler = subscribed,
            _ => { });

        handler!(null, EventArgs.Empty);
    }
}
