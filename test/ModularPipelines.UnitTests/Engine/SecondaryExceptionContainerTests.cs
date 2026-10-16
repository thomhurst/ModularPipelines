using System.Runtime.CompilerServices;
using ModularPipelines.Engine;

namespace ModularPipelines.UnitTests.Engine;

public class SecondaryExceptionContainerTests
{
    [Test]
    public async Task MultipleExceptions_AreThrownInRegistrationOrder()
    {
        var container = new SecondaryExceptionContainer();
        var first = new InvalidOperationException("first");
        var second = new ArgumentException("second");
        var third = new IOException("third");
        container.RegisterException(first);
        container.RegisterException(second);
        container.RegisterException(third);

        var aggregate = Assert.Throws<AggregateException>(container.ThrowExceptions);

        await Assert.That(aggregate!.InnerExceptions.Count).IsEqualTo(3);
        await Assert.That(aggregate.InnerExceptions[0]).IsSameReferenceAs(first);
        await Assert.That(aggregate.InnerExceptions[1]).IsSameReferenceAs(second);
        await Assert.That(aggregate.InnerExceptions[2]).IsSameReferenceAs(third);
    }

    [Test]
    public async Task ConcurrentRegistrations_PreserveEveryException()
    {
        var container = new SecondaryExceptionContainer();
        var exceptions = Enumerable.Range(0, 100)
            .Select(index => new InvalidOperationException(index.ToString()))
            .ToArray();

        await Task.WhenAll(exceptions.Select(exception => Task.Run(() => container.RegisterException(exception))));

        var aggregate = Assert.Throws<AggregateException>(container.ThrowExceptions);
        await Assert.That(aggregate!.InnerExceptions.Count).IsEqualTo(exceptions.Length);
        foreach (var exception in exceptions)
        {
            await Assert.That(aggregate.InnerExceptions.Contains(exception)).IsTrue();
        }
    }

    [Test]
    public async Task SingleException_IsRethrownWithOriginalStackTrace()
    {
        var container = new SecondaryExceptionContainer();
        var original = Assert.Throws<InvalidOperationException>(ThrowOriginalException);
        container.RegisterException(original!);

        var rethrown = Assert.Throws<InvalidOperationException>(container.ThrowExceptions);

        await Assert.That(rethrown).IsSameReferenceAs(original);
        await Assert.That(rethrown!.StackTrace).Contains(nameof(ThrowOriginalException));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowOriginalException() => throw new InvalidOperationException("original");
}
