using System.Net;

namespace ModularPipelines.Http;

internal sealed class TimeoutHttpContent : HttpContent
{
    private readonly HttpContent _innerContent;
    private readonly CancellationTokenSource _timeoutCancellationTokenSource;
    private readonly CancellationTokenSource _linkedCancellationTokenSource;

    public TimeoutHttpContent(
        HttpContent innerContent,
        CancellationTokenSource timeoutCancellationTokenSource,
        CancellationTokenSource linkedCancellationTokenSource)
    {
        _innerContent = innerContent;
        _timeoutCancellationTokenSource = timeoutCancellationTokenSource;
        _linkedCancellationTokenSource = linkedCancellationTokenSource;

        foreach (var header in innerContent.Headers)
        {
            Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        return _innerContent.CopyToAsync(stream, _linkedCancellationTokenSource.Token);
    }

    protected override async Task SerializeToStreamAsync(
        Stream stream,
        TransportContext? context,
        CancellationToken cancellationToken)
    {
        using var readCancellationTokenSource = CreateReadCancellationTokenSource(cancellationToken);
        await _innerContent.CopyToAsync(
                stream,
                readCancellationTokenSource?.Token ?? _linkedCancellationTokenSource.Token)
            .ConfigureAwait(false);
    }

    protected override async Task<Stream> CreateContentReadStreamAsync()
    {
        var stream = await _innerContent
            .ReadAsStreamAsync(_linkedCancellationTokenSource.Token)
            .ConfigureAwait(false);
        return new TimeoutReadStream(stream, _linkedCancellationTokenSource.Token);
    }

    protected override async Task<Stream> CreateContentReadStreamAsync(
        CancellationToken cancellationToken)
    {
        using var readCancellationTokenSource = CreateReadCancellationTokenSource(cancellationToken);
        var effectiveCancellationToken =
            readCancellationTokenSource?.Token ?? _linkedCancellationTokenSource.Token;
        var stream = await _innerContent
            .ReadAsStreamAsync(effectiveCancellationToken)
            .ConfigureAwait(false);
        return new TimeoutReadStream(stream, _linkedCancellationTokenSource.Token);
    }

    protected override bool TryComputeLength(out long length)
    {
        if (_innerContent.Headers.ContentLength is { } contentLength)
        {
            length = contentLength;
            return true;
        }

        length = 0;
        return false;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _innerContent.Dispose();
            _linkedCancellationTokenSource.Dispose();
            _timeoutCancellationTokenSource.Dispose();
        }

        base.Dispose(disposing);
    }

    private CancellationTokenSource? CreateReadCancellationTokenSource(
        CancellationToken cancellationToken)
    {
        return cancellationToken.CanBeCanceled &&
               cancellationToken != _linkedCancellationTokenSource.Token
            ? CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken,
                _linkedCancellationTokenSource.Token)
            : null;
    }

    private sealed class TimeoutReadStream(
        Stream innerStream,
        CancellationToken timeoutCancellationToken) : Stream
    {
        public override bool CanRead => innerStream.CanRead;

        public override bool CanSeek => innerStream.CanSeek;

        public override bool CanWrite => innerStream.CanWrite;

        public override long Length => innerStream.Length;

        public override long Position
        {
            get => innerStream.Position;
            set => innerStream.Position = value;
        }

        public override void Flush() => innerStream.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            return innerStream.FlushAsync(cancellationToken);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            timeoutCancellationToken.ThrowIfCancellationRequested();
            return innerStream.Read(buffer, offset, count);
        }

        public override int Read(Span<byte> buffer)
        {
            timeoutCancellationToken.ThrowIfCancellationRequested();
            return innerStream.Read(buffer);
        }

        public override Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            return ReadAsync(buffer.AsMemory(offset, count), cancellationToken).AsTask();
        }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            using var readCancellationTokenSource =
                CreateReadCancellationTokenSource(cancellationToken);
            return await innerStream.ReadAsync(
                    buffer,
                    readCancellationTokenSource?.Token ?? timeoutCancellationToken)
                .ConfigureAwait(false);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return innerStream.Seek(offset, origin);
        }

        public override void SetLength(long value) => innerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            innerStream.Write(buffer, offset, count);
        }

        public override async ValueTask DisposeAsync()
        {
            await innerStream.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                innerStream.Dispose();
            }

            base.Dispose(disposing);
        }

        private CancellationTokenSource? CreateReadCancellationTokenSource(
            CancellationToken cancellationToken)
        {
            return cancellationToken.CanBeCanceled &&
                   cancellationToken != timeoutCancellationToken
                ? CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken,
                    timeoutCancellationToken)
                : null;
        }
    }
}
