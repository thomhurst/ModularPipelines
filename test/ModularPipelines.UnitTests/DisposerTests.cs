using ModularPipelines.Helpers;

namespace ModularPipelines.UnitTests;

public class DisposerTests
{
    private class MyClass : IDisposable, IAsyncDisposable
    {
        public bool DisposedAsync { get; private set; }
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }

        public ValueTask DisposeAsync()
        {
            DisposedAsync = true;
            return ValueTask.CompletedTask;
        }
    }

    [Test]
    public async Task Disposer_Calls_Sync_And_Async()
    {
        var myClass = new MyClass();
        
        Assert.Multiple(() =>
        {
            Assert.That(myClass.Disposed, Is.False);
            Assert.That(myClass.DisposedAsync, Is.False);
        });
        
        await Disposer.DisposeObjectAsync(myClass);
        
        Assert.Multiple(() =>
        {
            Assert.That(myClass.Disposed, Is.True);
            Assert.That(myClass.DisposedAsync, Is.True);
        });
    }
}