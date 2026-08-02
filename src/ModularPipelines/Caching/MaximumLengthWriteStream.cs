namespace ModularPipelines.Caching;

internal sealed class MaximumLengthWriteStream(Stream inner, long maximumLength) : Stream
{
    private long _bytesWritten;

    public override bool CanRead => false;

    public override bool CanSeek => false;

    public override bool CanWrite => inner.CanWrite;

    public override long Length => _bytesWritten;

    public override long Position
    {
        get => _bytesWritten;
        set => throw new NotSupportedException();
    }

    public override void Flush() => inner.Flush();

    public override Task FlushAsync(CancellationToken cancellationToken) =>
        inner.FlushAsync(cancellationToken);

    public override int Read(byte[] buffer, int offset, int count) =>
        throw new NotSupportedException();

    public override long Seek(long offset, SeekOrigin origin) =>
        throw new NotSupportedException();

    public override void SetLength(long value) =>
        throw new NotSupportedException();

    public override void Write(byte[] buffer, int offset, int count)
    {
        EnsureWithinLimit(count);
        inner.Write(buffer, offset, count);
        _bytesWritten += count;
    }

    public override void Write(ReadOnlySpan<byte> buffer)
    {
        EnsureWithinLimit(buffer.Length);
        inner.Write(buffer);
        _bytesWritten += buffer.Length;
    }

    public override async Task WriteAsync(
        byte[] buffer,
        int offset,
        int count,
        CancellationToken cancellationToken)
    {
        EnsureWithinLimit(count);
        await inner.WriteAsync(buffer.AsMemory(offset, count), cancellationToken)
            .ConfigureAwait(false);
        _bytesWritten += count;
    }

    public override async ValueTask WriteAsync(
        ReadOnlyMemory<byte> buffer,
        CancellationToken cancellationToken = default)
    {
        EnsureWithinLimit(buffer.Length);
        await inner.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
        _bytesWritten += buffer.Length;
    }

    public override void WriteByte(byte value)
    {
        EnsureWithinLimit(1);
        inner.WriteByte(value);
        _bytesWritten++;
    }

    private void EnsureWithinLimit(int byteCount)
    {
        if (_bytesWritten > maximumLength - byteCount)
        {
            throw new MaximumLengthExceededException(maximumLength);
        }
    }
}

internal sealed class MaximumLengthExceededException(long maximumLength)
    : IOException($"Stream exceeded the configured limit of {maximumLength:N0} bytes.");
