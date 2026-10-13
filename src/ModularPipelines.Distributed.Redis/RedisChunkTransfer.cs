using System.Buffers;
using Respire;

namespace ModularPipelines.Distributed.Redis;

/// <summary>
/// Moves chunked values with several commands in flight. Each chunk is one round trip, so a sequential
/// transfer spends most of its time waiting on latency; a small window keeps the link busy without letting
/// one transfer flood the connection.
/// </summary>
internal static class RedisChunkTransfer
{
    /// <summary>The number of chunk commands a single transfer keeps in flight.</summary>
    internal const int MaxInFlight = 4;

    /// <summary>
    /// Writes each buffer from <paramref name="chunks"/> with <paramref name="write"/>, keeping up to
    /// <see cref="MaxInFlight"/> writes running. Every buffer must be owned by its write: none is reused.
    /// </summary>
    /// <returns>The number of chunks written.</returns>
    public static async Task<int> WriteAsync(
        IAsyncEnumerable<byte[]> chunks,
        Func<int, byte[], CancellationToken, Task> write,
        CancellationToken cancellationToken)
    {
        var inFlight = new Queue<Task>(MaxInFlight);
        var count = 0;
        try
        {
            await foreach (var chunk in chunks.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                if (inFlight.Count == MaxInFlight)
                {
                    await inFlight.Dequeue().ConfigureAwait(false);
                }

                inFlight.Enqueue(write(count, chunk, cancellationToken));
                count++;
            }

            while (inFlight.Count > 0)
            {
                await inFlight.Dequeue().ConfigureAwait(false);
            }
        }
        catch
        {
            // Let the remaining writes finish so their failures are observed and no buffer is still borrowed.
            await ObserveAsync(inFlight).ConfigureAwait(false);
            throw;
        }

        return count;
    }

    /// <summary>
    /// Copies chunks <c>0, 1, 2, …</c> from <paramref name="read"/> into <paramref name="destination"/> in order,
    /// keeping up to <see cref="MaxInFlight"/> reads running, until a chunk is missing or
    /// <paramref name="chunkLimit"/> chunks were copied. <paramref name="reject"/> can refuse a chunk, given its
    /// length, before it is written by returning the exception to throw.
    /// </summary>
    /// <returns>The number of chunks copied.</returns>
    public static async Task<int> ReadAsync(
        Func<int, CancellationToken, Task<RespireLease>> read,
        Stream destination,
        int chunkLimit,
        Func<int, Exception?>? reject,
        CancellationToken cancellationToken)
    {
        var inFlight = new Queue<Task<RespireLease>>(MaxInFlight);
        var next = 0;
        var copied = 0;
        try
        {
            while (true)
            {
                while (inFlight.Count < MaxInFlight && next < chunkLimit)
                {
                    inFlight.Enqueue(read(next++, cancellationToken));
                }

                if (inFlight.Count == 0)
                {
                    return copied;
                }

                using var lease = await inFlight.Dequeue().ConfigureAwait(false);
                if (lease.IsNull)
                {
                    await ReleaseAsync(inFlight).ConfigureAwait(false);
                    return copied;
                }

                if (reject?.Invoke(lease.Length) is { } rejection)
                {
                    throw rejection;
                }

                await WriteLeaseAsync(lease, destination, cancellationToken).ConfigureAwait(false);
                copied++;
            }
        }
        catch
        {
            await ReleaseAsync(inFlight).ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>Reads <paramref name="source"/> into new buffers of at most <paramref name="chunkSize"/> bytes.</summary>
    public static async IAsyncEnumerable<byte[]> ReadChunksAsync(
        Stream source,
        int chunkSize,
        byte[]? first,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (first is not null)
        {
            yield return first;
            if (first.Length < chunkSize)
            {
                yield break;
            }
        }

        while (true)
        {
            var chunk = await ReadChunkAsync(source, chunkSize, cancellationToken).ConfigureAwait(false);
            if (chunk.Length > 0)
            {
                yield return chunk;
            }

            if (chunk.Length < chunkSize)
            {
                yield break;
            }
        }
    }

    /// <summary>Reads up to <paramref name="chunkSize"/> bytes, returning fewer only at the end of the stream.</summary>
    public static async Task<byte[]> ReadChunkAsync(Stream source, int chunkSize, CancellationToken cancellationToken)
    {
        var buffer = new byte[chunkSize];
        var totalRead = 0;
        while (totalRead < buffer.Length)
        {
            var read = await source.ReadAsync(buffer.AsMemory(totalRead), cancellationToken).ConfigureAwait(false);
            if (read == 0)
            {
                break;
            }

            totalRead += read;
        }

        return totalRead == buffer.Length ? buffer : buffer[..totalRead];
    }

    // Leases expose only a span, so stage each one in a pooled buffer for the asynchronous write.
    private static async Task WriteLeaseAsync(RespireLease lease, Stream destination, CancellationToken cancellationToken)
    {
        var buffer = ArrayPool<byte>.Shared.Rent(lease.Length);
        try
        {
            lease.Span.CopyTo(buffer);
            await destination.WriteAsync(buffer.AsMemory(0, lease.Length), cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static async Task ReleaseAsync(Queue<Task<RespireLease>> inFlight)
    {
        while (inFlight.Count > 0)
        {
            try
            {
                using var lease = await inFlight.Dequeue().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // The transfer is already ending; this read's failure no longer matters.
            }
        }
    }

    private static async Task ObserveAsync(Queue<Task> inFlight)
    {
        try
        {
            await Task.WhenAll(inFlight).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // The first failure is already being rethrown.
        }
    }
}
