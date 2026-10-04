namespace ModularPipelines.UnitTests.Helpers;

internal sealed class AsyncIoTestStream(MemoryStream inner) : Stream
{
    public Action<CancellationToken>? BeforeRead { get; init; }
    public Action<CancellationToken>? BeforeWrite { get; init; }
    public bool AllowSynchronousMetadataReads { get; init; }
    public bool AllowSynchronousArchiveWrites { get; init; }
    public int AsyncReads { get; private set; }
    public int AsyncWrites { get; private set; }
    public bool DisposedAsynchronously { get; private set; }
    public byte[] Bytes => inner.ToArray();
    public override bool CanRead => inner.CanRead;
    public override bool CanSeek => inner.CanSeek;
    public override bool CanWrite => inner.CanWrite;
    public override long Length => inner.Length;
    public override long Position { get => inner.Position; set => inner.Position = value; }
    public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);
    public override void SetLength(long value) => inner.SetLength(value);
    public override void Flush()
    {
        if (!AllowSynchronousArchiveWrites)
        {
            throw new InvalidOperationException("Synchronous flush is not allowed.");
        }

        inner.Flush();
    }
    public override int Read(byte[] buffer, int offset, int count) => AllowSynchronousMetadataReads
        ? inner.Read(buffer, offset, count)
        : throw new InvalidOperationException("Synchronous read is not allowed.");
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (!AllowSynchronousArchiveWrites)
        {
            throw new InvalidOperationException("Synchronous write is not allowed.");
        }

        inner.Write(buffer, offset, count);
    }
    public override Task FlushAsync(CancellationToken cancellationToken) => inner.FlushAsync(cancellationToken);

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        AsyncReads++;
        BeforeRead?.Invoke(cancellationToken);
        return inner.ReadAsync(buffer, cancellationToken);
    }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
    {
        AsyncWrites++;
        BeforeWrite?.Invoke(cancellationToken);
        return inner.WriteAsync(buffer, cancellationToken);
    }

    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
        WriteAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();

    public override ValueTask DisposeAsync()
    {
        DisposedAsynchronously = true;
        return inner.DisposeAsync();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            inner.Dispose();
        }

        base.Dispose(disposing);
    }
}
