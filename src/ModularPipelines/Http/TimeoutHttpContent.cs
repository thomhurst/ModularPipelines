using System.Net;

namespace ModularPipelines.Http;

internal sealed class TimeoutHttpContent : HttpContent
{
    private readonly HttpContent _innerContent;
    private readonly CancellationTokenSource? _timeoutCancellationTokenSource;
    private readonly CancellationTokenSource _linkedCancellationTokenSource;

    public TimeoutHttpContent(
        HttpContent innerContent,
        CancellationTokenSource? timeoutCancellationTokenSource,
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
        return CopyInnerContentToAsync(stream, CancellationToken.None);
    }

    protected override async Task SerializeToStreamAsync(
        Stream stream,
        TransportContext? context,
        CancellationToken cancellationToken)
    {
        await CopyInnerContentToAsync(stream, cancellationToken).ConfigureAwait(false);
    }

    protected override async Task<Stream> CreateContentReadStreamAsync()
    {
        _linkedCancellationTokenSource.Token.ThrowIfCancellationRequested();
        var stream = await _innerContent
            .ReadAsStreamAsync(_linkedCancellationTokenSource.Token)
            .ConfigureAwait(false);
        return new TimeoutReadStream(stream, _linkedCancellationTokenSource.Token);
    }

    protected override Stream CreateContentReadStream(CancellationToken cancellationToken)
    {
        using var readCancellationTokenSource = CreateReadCancellationTokenSource(cancellationToken);
        var effectiveCancellationToken =
            readCancellationTokenSource?.Token ?? _linkedCancellationTokenSource.Token;
        effectiveCancellationToken.ThrowIfCancellationRequested();
        var stream = _innerContent.ReadAsStream(effectiveCancellationToken);
        return new TimeoutReadStream(stream, _linkedCancellationTokenSource.Token);
    }

    protected override async Task<Stream> CreateContentReadStreamAsync(
        CancellationToken cancellationToken)
    {
        using var readCancellationTokenSource = CreateReadCancellationTokenSource(cancellationToken);
        var effectiveCancellationToken =
            readCancellationTokenSource?.Token ?? _linkedCancellationTokenSource.Token;
        effectiveCancellationToken.ThrowIfCancellationRequested();
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
            _timeoutCancellationTokenSource?.Dispose();
        }

        base.Dispose(disposing);
    }

    private async Task CopyInnerContentToAsync(
        Stream destination,
        CancellationToken cancellationToken)
    {
        using var readCancellationTokenSource = CreateReadCancellationTokenSource(cancellationToken);
        var effectiveCancellationToken =
            readCancellationTokenSource?.Token ?? _linkedCancellationTokenSource.Token;
        effectiveCancellationToken.ThrowIfCancellationRequested();
        var source = await _innerContent
            .ReadAsStreamAsync(effectiveCancellationToken)
            .ConfigureAwait(false);
        using var cancellationRegistration = effectiveCancellationToken.Register(
            static state => ((Stream) state!).Dispose(),
            source);

        try
        {
            await source.CopyToAsync(destination, effectiveCancellationToken).ConfigureAwait(false);
            effectiveCancellationToken.ThrowIfCancellationRequested();
        }
        catch (Exception exception)
            when (effectiveCancellationToken.IsCancellationRequested
                  && exception is ObjectDisposedException or IOException)
        {
            throw new OperationCanceledException(
                "The HTTP response body copy was cancelled.",
                exception,
                effectiveCancellationToken);
        }
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
        private readonly CancellationTokenRegistration _timeoutRegistration =
            timeoutCancellationToken.Register(
                static state => ((Stream) state!).Dispose(),
                innerStream);

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
            try
            {
                return innerStream.Read(buffer, offset, count);
            }
            catch (Exception exception)
                when (timeoutCancellationToken.IsCancellationRequested &&
                      exception is ObjectDisposedException or IOException)
            {
                throw CreateTimeoutException(exception);
            }
        }

        public override int Read(Span<byte> buffer)
        {
            timeoutCancellationToken.ThrowIfCancellationRequested();
            try
            {
                return innerStream.Read(buffer);
            }
            catch (Exception exception)
                when (timeoutCancellationToken.IsCancellationRequested &&
                      exception is ObjectDisposedException or IOException)
            {
                throw CreateTimeoutException(exception);
            }
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
            var effectiveCancellationToken =
                readCancellationTokenSource?.Token ?? timeoutCancellationToken;
            effectiveCancellationToken.ThrowIfCancellationRequested();
            using var cancellationRegistration = readCancellationTokenSource?.Token.Register(
                static state => ((Stream) state!).Dispose(),
                innerStream);
            try
            {
                var bytesRead = await innerStream.ReadAsync(
                        buffer,
                        effectiveCancellationToken)
                    .ConfigureAwait(false);
                effectiveCancellationToken.ThrowIfCancellationRequested();
                return bytesRead;
            }
            catch (Exception exception)
                when (effectiveCancellationToken.IsCancellationRequested &&
                      exception is ObjectDisposedException or IOException)
            {
                throw CreateReadCancellationException(
                    exception,
                    effectiveCancellationToken);
            }
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
            _timeoutRegistration.Dispose();
            await innerStream.DisposeAsync().ConfigureAwait(false);
            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timeoutRegistration.Dispose();
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

        private OperationCanceledException CreateTimeoutException(Exception exception)
        {
            return new OperationCanceledException(
                "The HTTP response body read timed out.",
                exception,
                timeoutCancellationToken);
        }

        private static OperationCanceledException CreateReadCancellationException(
            Exception exception,
            CancellationToken cancellationToken)
        {
            return new OperationCanceledException(
                "The HTTP response body read was cancelled.",
                exception,
                cancellationToken);
        }
    }
}
