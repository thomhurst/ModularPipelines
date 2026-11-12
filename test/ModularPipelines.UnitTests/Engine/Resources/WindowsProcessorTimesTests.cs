using System.Runtime.CompilerServices;
using ModularPipelines.Engine.Resources;

namespace ModularPipelines.UnitTests.Engine.Resources;

public class WindowsProcessorTimesTests
{
    [Test]
    [Arguments(1, 640L, 1600L)]
    [Arguments(2, 662L, 1652L)]
    public async Task TryRead_AggregatesEveryProcessorInEveryGroup(int groupCount, long expectedIdle, long expectedTotal)
    {
        var groups = new List<ushort>();
        bool Read(ushort group, Span<WindowsProcessorTimes.ProcessorPerformance> buffer, out uint bytes)
        {
            groups.Add(group);
            // A full group followed by a smaller group exercises unequal group sizes.
            var count = group == 0 ? 64 : 2;
            for (var index = 0; index < count; index++)
            {
                buffer[index] = new() { IdleTime = 10 + group, KernelTime = 20 + group, UserTime = 5 };
            }

            bytes = (uint) count * (uint) Unsafe.SizeOf<WindowsProcessorTimes.ProcessorPerformance>();
            return true;
        }

        var success = WindowsProcessorTimes.TryRead((ushort) groupCount, Read, out var idle, out var total);
        await Assert.That(success).IsTrue();
        await Assert.That(groups).IsEquivalentTo(Enumerable.Range(0, groupCount).Select(group => (ushort) group));
        await Assert.That(idle).IsEqualTo(expectedIdle);
        await Assert.That(total).IsEqualTo(expectedTotal);
    }

    [Test]
    [Arguments(false, 48u)]
    [Arguments(true, 0u)]
    [Arguments(true, 47u)]
    [Arguments(true, 3120u)]
    public async Task TryRead_RejectsFailedOrMalformedGroupWithoutPublishingPartialTotals(bool success, uint returnedBytes)
    {
        bool Read(ushort group, Span<WindowsProcessorTimes.ProcessorPerformance> buffer, out uint bytes)
        {
            buffer[0] = new() { IdleTime = 10, KernelTime = 20, UserTime = 5 };
            bytes = group == 0 ? 48u : returnedBytes;
            return group == 0 || success;
        }

        var result = WindowsProcessorTimes.TryRead(2, Read, out var idle, out var total);
        await Assert.That(result).IsFalse();
        await Assert.That(idle).IsEqualTo(0L);
        await Assert.That(total).IsEqualTo(0L);
    }

    [Test]
    public async Task TryRead_RejectsMissingGroups()
    {
        var called = false;
        bool Read(ushort group, Span<WindowsProcessorTimes.ProcessorPerformance> buffer, out uint bytes)
        {
            called = true;
            bytes = 0;
            return true;
        }

        var result = WindowsProcessorTimes.TryRead(0, Read, out var idle, out var total);
        await Assert.That(result).IsFalse();
        await Assert.That(called).IsFalse();
        await Assert.That(idle).IsEqualTo(0L);
        await Assert.That(total).IsEqualTo(0L);
    }
}
