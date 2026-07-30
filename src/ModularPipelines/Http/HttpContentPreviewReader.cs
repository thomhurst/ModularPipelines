using System.Text;

namespace ModularPipelines.Http;

/// <summary>
/// Reads bounded text previews while preserving the complete content stream for its consumer.
/// </summary>
internal static class HttpContentPreviewReader
{
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
            // ReadAsStringAsync buffers HttpContent, so returning the same instance preserves replay.
            var unboundedBody = await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            return (unboundedBody, false, content, content.Headers.ContentLength);
        }

        var stream = await content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var buffer = new byte[maxBytes + 1];
        var bytesRead = 0;

        while (bytesRead < buffer.Length)
        {
            var read = await stream.ReadAsync(
                    buffer.AsMemory(bytesRead, buffer.Length - bytesRead),
                    cancellationToken)
                .ConfigureAwait(false);
            if (read == 0)
            {
                break;
            }

            bytesRead += read;
        }

        var isTruncated = bytesRead > maxBytes;
        var previewLength = Math.Min(bytesRead, maxBytes);
        var encoding = GetEncoding(content);
        var preview = DecodeCompletePrefix(encoding, buffer, previewLength);
        var replayContent = CreateReplayContent(content, stream, buffer.AsSpan(0, bytesRead).ToArray());

        return (preview, isTruncated, replayContent, content.Headers.ContentLength);
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
        foreach (var header in originalContent.Headers)
        {
            replayContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return replayContent;
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
