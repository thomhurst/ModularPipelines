using System.Text;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Commands;

public class CommandInputEncodingTests
{
    [Test]
    public async Task Writes_Utf8_In_Bounded_Chunks_Without_Splitting_Surrogates()
    {
        var input = new string('x', 4095) + "🚀東京" + new string('é', 8192);
        using var stream = new TrackingStream();

        await ProcessRunner.WriteInputAsync(stream, input, CancellationToken.None);

        await Assert.That(stream.ToArray()).IsEquivalentTo(Encoding.UTF8.GetBytes(input));
        await Assert.That(stream.MaximumWriteSize).IsLessThanOrEqualTo(4096);
    }

    [Test]
    public async Task Stops_Encoding_After_Cancellation_Between_Writes()
    {
        using var cancellation = new CancellationTokenSource();
        using var stream = new TrackingStream(cancellation);

        await Assert.ThrowsAsync<OperationCanceledException>(() => ProcessRunner.WriteInputAsync(
            stream, new string('x', 128 * 1024), cancellation.Token));

        await Assert.That(stream.Length).IsEqualTo(4096L);
    }

    private sealed class TrackingStream(CancellationTokenSource? cancellation = null) : MemoryStream
    {
        public int MaximumWriteSize { get; private set; }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            MaximumWriteSize = Math.Max(MaximumWriteSize, buffer.Length);
            var write = base.WriteAsync(buffer, cancellationToken);
            cancellation?.Cancel();
            return write;
        }
    }
}
