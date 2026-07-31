using System.Buffers;
using System.Text;

namespace ModularPipelines.Http;

/// <summary>
/// Reads bounded text previews while preserving the complete content stream for its consumer.
/// </summary>
internal static class HttpContentPreviewReader
{
    private const int MaximumPreviewBufferSize = 81920;

    /// <summary>
    /// Reads at most one byte beyond the configured preview limit.
    /// </summary>
    /// <param name="content">The HTTP content to preview.</param>
    /// <param name="maxBytes">The maximum number of bytes to include in the preview.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The preview, truncation state, replayable content, and declared total length.</returns>
    public static async Task<(string Preview, bool IsTruncated, HttpContent ReplayContent, long? TotalLength)>
        ReadAsync(
            HttpContent content,
            int maxBytes,
            CancellationToken cancellationToken)
    {
        if (maxBytes <= 0 || maxBytes == int.MaxValue)
        {
            var totalLength = content.Headers.ContentLength;
            var bytes = await ReadAllBytesAsync(content, cancellationToken).ConfigureAwait(false);
            var unboundedReplayContent = new ByteArrayContent(bytes);
            CopyHeaders(content, unboundedReplayContent);
            content.Dispose();
            return (DecodeCompletePrefix(GetEncoding(unboundedReplayContent), bytes, bytes.Length),
                false,
                unboundedReplayContent,
                totalLength ?? bytes.LongLength);
        }

        var stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var cancellationRegistration = cancellationToken.Register(
            static state => ((Stream) state!).Dispose(),
            stream);
        var probeLength = maxBytes + 1;
        var readBuffer = ArrayPool<byte>.Shared.Rent(
            Math.Min(probeLength, MaximumPreviewBufferSize));
        using var buffer = new MemoryStream(GetInitialCapacity(content, probeLength));

        try
        {
            while (buffer.Length < probeLength)
            {
                var bytesRemaining = probeLength - (int) buffer.Length;
                var read = await stream.ReadAsync(
                        readBuffer.AsMemory(
                            0,
                            Math.Min(MaximumPreviewBufferSize, bytesRemaining)),
                        cancellationToken)
                    .ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                if (read == 0)
                {
                    break;
                }

                buffer.Write(readBuffer, 0, read);
            }
        }
        catch (Exception exception)
            when (cancellationToken.IsCancellationRequested
                  && exception is ObjectDisposedException or IOException)
        {
            throw new OperationCanceledException(
                "The HTTP content preview read was cancelled.",
                exception,
                cancellationToken);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(readBuffer);
        }

        var bufferedBytes = buffer.ToArray();
        var bytesRead = bufferedBytes.Length;
        var isTruncated = bytesRead > maxBytes;
        var previewLength = Math.Min(bytesRead, maxBytes);
        var encoding = GetEncoding(content);
        var preview = DecodeCompletePrefix(encoding, bufferedBytes, previewLength);
        var replayContent = CreateReplayContent(content, stream, bufferedBytes);

        return (preview, isTruncated, replayContent, content.Headers.ContentLength);
    }

    /// <summary>
    /// Reads request content into replayable storage so redirects and authentication retries can resend it.
    /// </summary>
    /// <param name="content">The HTTP request content to preview.</param>
    /// <param name="maxBytes">The maximum number of bytes to include in the preview.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The preview, truncation state, replayable content, and total length.</returns>
    /// <remarks>This method takes ownership of and disposes <paramref name="content"/>.</remarks>
    public static async Task<(string Preview, bool IsTruncated, HttpContent ReplayContent, long? TotalLength)>
        ReadReplayableAsync(
            HttpContent content,
            int maxBytes,
            CancellationToken cancellationToken)
    {
        var totalLength = content.Headers.ContentLength;
        var bytes = await ReadAllBytesAsync(content, cancellationToken).ConfigureAwait(false);
        var previewLength = maxBytes <= 0 || maxBytes == int.MaxValue
            ? bytes.Length
            : Math.Min(bytes.Length, maxBytes);
        var isTruncated = previewLength < bytes.Length;
        var preview = DecodeCompletePrefix(GetEncoding(content), bytes, previewLength);
        var replayContent = new ByteArrayContent(bytes);
        CopyHeaders(content, replayContent);
        content.Dispose();

        return (
            preview,
            isTruncated,
            replayContent,
            totalLength ?? bytes.LongLength);
    }

    private static async Task<byte[]> ReadAllBytesAsync(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        var stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var cancellationRegistration = cancellationToken.Register(
            static state => ((Stream) state!).Dispose(),
            stream);
        using var buffer = new MemoryStream();

        try
        {
            await stream.CopyToAsync(buffer, cancellationToken).ConfigureAwait(false);
            return buffer.ToArray();
        }
        catch (Exception exception)
            when (cancellationToken.IsCancellationRequested
                  && exception is ObjectDisposedException or IOException)
        {
            throw new OperationCanceledException(
                "The HTTP content read was cancelled.",
                exception,
                cancellationToken);
        }
    }

    private static int GetInitialCapacity(HttpContent content, int probeLength)
    {
        var declaredLength = content.Headers.ContentLength;
        return declaredLength is >= 0
            ? (int) Math.Min(Math.Min(declaredLength.Value, probeLength), MaximumPreviewBufferSize)
            : Math.Min(probeLength, MaximumPreviewBufferSize);
    }

    private static Encoding GetEncoding(HttpContent content)
    {
        var charset = content.Headers.ContentType?.CharSet?.Trim('"');
        if (string.IsNullOrWhiteSpace(charset))
        {
            return Encoding.UTF8;
        }

        try
        {
            return Encoding.GetEncoding(charset);
        }
        catch (ArgumentException)
        {
            return Encoding.UTF8;
        }
    }

    private static string DecodeCompletePrefix(Encoding encoding, byte[] buffer, int length)
    {
        var decoder = encoding.GetDecoder();
        var characters = new char[encoding.GetMaxCharCount(length)];
        decoder.Convert(
            buffer.AsSpan(0, length),
            characters,
            flush: false,
            out _,
            out var charactersUsed,
            out _);
        return new string(characters, 0, charactersUsed);
    }

    private static StreamContent CreateReplayContent(
        HttpContent originalContent,
        Stream remainingStream,
        byte[] prefix)
    {
        var replayContent = new StreamContent(
            new PrefixReplayStream(prefix, remainingStream, originalContent));
        CopyHeaders(originalContent, replayContent);

        return replayContent;
    }

    private static void CopyHeaders(HttpContent source, HttpContent destination)
    {
        foreach (var header in source.Headers)
        {
            destination.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    private sealed class PrefixReplayStream(
        byte[] prefix,
        Stream remainingStream,
        HttpContent ownedContent) : Stream
    {
        private int _prefixPosition;

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var prefixBytesRead = ReadPrefix(buffer.AsSpan(offset, count));
            return prefixBytesRead > 0
                ? prefixBytesRead
                : remainingStream.Read(buffer, offset, count);
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
            var prefixBytesRead = ReadPrefix(buffer.Span);
            return prefixBytesRead > 0
                ? prefixBytesRead
                : await remainingStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ownedContent.Dispose();
            }

            base.Dispose(disposing);
        }

        private int ReadPrefix(Span<byte> destination)
        {
            var bytesToCopy = Math.Min(destination.Length, prefix.Length - _prefixPosition);
            if (bytesToCopy <= 0)
            {
                return 0;
            }

            prefix.AsSpan(_prefixPosition, bytesToCopy).CopyTo(destination);
            _prefixPosition += bytesToCopy;
            return bytesToCopy;
        }
    }
}
