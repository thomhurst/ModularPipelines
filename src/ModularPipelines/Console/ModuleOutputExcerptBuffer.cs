using System.Text;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Console;

internal sealed class ModuleOutputExcerptBuffer(
    int maximumBytes,
    ISecretObfuscator? secretObfuscator = null,
    ISecretProvider? secretProvider = null)
{
    private static readonly Encoding Utf8 = new UTF8Encoding(
        encoderShouldEmitUTF8Identifier: false,
        throwOnInvalidBytes: false);
    private static readonly byte[] NewLineBytes = Utf8.GetBytes(Environment.NewLine);
    private readonly LinkedList<OutputChunk> _chunks = [];
    private readonly int _retentionBytes = maximumBytes > int.MaxValue / 2
        ? int.MaxValue
        : maximumBytes * 2;
    private long _totalBytes;
    private long _totalStdoutBytes;
    private long _totalStderrBytes;
    private int _retainedBytes;

    public void Append(string value, ModuleOutputStream stream)
    {
        var appendedBytes = (long) Utf8.GetByteCount(value) + NewLineBytes.Length;
        _totalBytes += appendedBytes;
        if (stream is ModuleOutputStream.StandardError)
        {
            _totalStderrBytes += appendedBytes;
        }
        else
        {
            _totalStdoutBytes += appendedBytes;
        }

        // One UTF-16 code unit always produces at least one UTF-8 byte. Keep one extra output
        // window so late-registered secrets can be masked before the final tail is selected.
        var start = Math.Max(0, value.Length - _retentionBytes);
        if (start > 0
            && char.IsLowSurrogate(value[start])
            && char.IsHighSurrogate(value[start - 1]))
        {
            start--;
        }

        var retainedValue = value.AsSpan(start);
        var valueByteCount = Utf8.GetByteCount(retainedValue);
        var bytes = new byte[valueByteCount + NewLineBytes.Length];
        Utf8.GetBytes(retainedValue, bytes);
        NewLineBytes.CopyTo(bytes, valueByteCount);
        _chunks.AddLast(new OutputChunk(stream, bytes));
        _retainedBytes += bytes.Length;
        TrimToLimit(_retentionBytes);
    }

    public ModuleOutputExcerpt? CreateExcerpt()
    {
        if (_chunks.Count == 0)
        {
            return null;
        }

        var secretPatternsVersion = secretProvider?.Version;

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();
        foreach (var chunk in _chunks)
        {
            (chunk.Stream is ModuleOutputStream.StandardError ? stderr : stdout)
                .Append(Utf8.GetString(chunk.Bytes));
        }

        var stdoutValue = stdout.ToString();
        var stderrValue = stderr.ToString();
        var (stdoutBytes, stderrBytes) = GetFinalStreamByteLimits();
        var truncatedBytes = GetTruncatedByteCount(
            stdoutValue,
            stderrValue,
            stdoutBytes,
            stderrBytes);
        if (!TryCreateTails(
                stdoutValue,
                stderrValue,
                stdoutBytes,
                stderrBytes,
                out var stdoutTail,
                out var stderrTail))
        {
            return null;
        }

        if (secretPatternsVersion is { } version && secretProvider?.Version != version)
        {
            return null;
        }

        return new ModuleOutputExcerpt
        {
            StdoutTail = stdoutTail,
            StderrTail = stderrTail,
            TruncatedBytes = truncatedBytes,
            SecretPatternsVersion = secretPatternsVersion,
        };
    }

    private long GetTruncatedByteCount(
        string stdout,
        string stderr,
        int stdoutBytes,
        int stderrBytes) =>
        Math.Max(
            0,
            _totalBytes
            - Utf8.GetByteCount(GetUtf8Tail(stdout, stdoutBytes) ?? string.Empty)
            - Utf8.GetByteCount(GetUtf8Tail(stderr, stderrBytes) ?? string.Empty));

    private bool TryCreateTails(
        string stdout,
        string stderr,
        int stdoutBytes,
        int stderrBytes,
        out string? stdoutTail,
        out string? stderrTail)
    {
        if (secretObfuscator is null && secretProvider is null)
        {
            stdoutTail = GetUtf8Tail(stdout, stdoutBytes);
            stderrTail = GetUtf8Tail(stderr, stderrBytes);
            return true;
        }

        return TryCreateMaskedTails(
            stdout,
            stderr,
            stdoutBytes,
            stderrBytes,
            out stdoutTail,
            out stderrTail);
    }

    private bool TryCreateMaskedTails(
        string stdout,
        string stderr,
        int stdoutBytes,
        int stderrBytes,
        out string? stdoutTail,
        out string? stderrTail)
    {
        stdoutTail = null;
        stderrTail = null;

        // Custom or incomplete masking dependencies do not expose enough information
        // to prove that a bounded context is safe.
        if (secretObfuscator is not SecretObfuscator concreteObfuscator || secretProvider is null)
        {
            return false;
        }

        var caseInsensitive = concreteObfuscator.CaseInsensitive;
        var snapshot = secretProvider.GetSnapshot();
        var maximumMatchBytes = snapshot.Secrets
            .Where(static secret => !string.IsNullOrEmpty(secret))
            .Select(secret => GetMaximumMatchByteCount(secret, caseInsensitive))
            .DefaultIfEmpty()
            .Max();
        if (maximumMatchBytes > maximumBytes)
        {
            return false;
        }

        var maskedStdout = secretObfuscator.Obfuscate(stdout, null);
        var maskedStderr = secretObfuscator.Obfuscate(stderr, null);
        (stdoutBytes, stderrBytes) = RebalanceMaskedStreamByteLimits(
            Utf8.GetByteCount(maskedStdout),
            Utf8.GetByteCount(maskedStderr),
            concreteObfuscator);

        if (!TryGetSafeMaskedTail(
                maskedStdout,
                stdoutBytes,
                _totalStdoutBytes > Utf8.GetByteCount(stdout),
                maximumMatchBytes,
                out stdoutTail)
            || !TryGetSafeMaskedTail(
                maskedStderr,
                stderrBytes,
                _totalStderrBytes > Utf8.GetByteCount(stderr),
                maximumMatchBytes,
                out stderrTail))
        {
            stdoutTail = stderrTail = null;
            return false;
        }

        // If registration changed during masking, a new secret may cross the retained
        // boundary. Omit the excerpt rather than risk returning a partial secret.
        return secretProvider.Version == snapshot.Version
               && concreteObfuscator.CaseInsensitive == caseInsensitive;
    }

    private (int StdoutBytes, int StderrBytes) RebalanceMaskedStreamByteLimits(
        int maskedStdoutBytes,
        int maskedStderrBytes,
        ISecretObfuscator obfuscator)
    {
        var stdoutBytes = 0;
        var stderrBytes = 0;
        var remaining = maximumBytes;
        var stdoutNeeded = maskedStdoutBytes;
        var stderrNeeded = maskedStderrBytes;

        for (var chunk = _chunks.Last; chunk is not null && remaining > 0; chunk = chunk.Previous)
        {
            var maskedChunkBytes = Utf8.GetByteCount(
                obfuscator.Obfuscate(Utf8.GetString(chunk.Value.Bytes), null));
            if (chunk.Value.Stream is ModuleOutputStream.StandardError && stderrNeeded > 0)
            {
                var added = Math.Min(Math.Min(stderrNeeded, maskedChunkBytes), remaining);
                stderrBytes += added;
                stderrNeeded -= added;
                remaining -= added;
            }
            else if (chunk.Value.Stream is ModuleOutputStream.StandardOutput && stdoutNeeded > 0)
            {
                var added = Math.Min(Math.Min(stdoutNeeded, maskedChunkBytes), remaining);
                stdoutBytes += added;
                stdoutNeeded -= added;
                remaining -= added;
            }
        }

        return (stdoutBytes, stderrBytes);
    }

    private static bool TryGetSafeMaskedTail(
        string maskedOutput,
        int maximumTailBytes,
        bool rawPrefixWasTrimmed,
        int maximumMatchBytes,
        out string? tail)
    {
        tail = GetUtf8Tail(maskedOutput, maximumTailBytes);
        if (!rawPrefixWasTrimmed || maximumTailBytes == 0)
        {
            return true;
        }

        // Mask contraction must not pull the selected tail within one possible
        // cross-boundary match of the discarded raw prefix.
        var discardedMaskedBytes = Utf8.GetByteCount(maskedOutput)
                                   - Utf8.GetByteCount(tail ?? string.Empty);
        return discardedMaskedBytes >= maximumMatchBytes;
    }

    private (int StdoutBytes, int StderrBytes) GetFinalStreamByteLimits()
    {
        var stdoutBytes = 0;
        var stderrBytes = 0;
        var remaining = maximumBytes;
        for (var chunk = _chunks.Last; chunk is not null && remaining > 0; chunk = chunk.Previous)
        {
            var retained = Math.Min(chunk.Value.Bytes.Length, remaining);
            if (chunk.Value.Stream is ModuleOutputStream.StandardError)
            {
                stderrBytes += retained;
            }
            else
            {
                stdoutBytes += retained;
            }

            remaining -= retained;
        }

        return (stdoutBytes, stderrBytes);
    }

    internal static string? GetUtf8Tail(string value, int maximumTailBytes)
    {
        if (maximumTailBytes == 0 || value.Length == 0)
        {
            return null;
        }

        var bytes = Utf8.GetBytes(value);
        if (bytes.Length <= maximumTailBytes)
        {
            return value;
        }

        var start = bytes.Length - maximumTailBytes;
        while (start < bytes.Length && IsUtf8ContinuationByte(bytes[start]))
        {
            start++;
        }

        return start == bytes.Length ? null : Utf8.GetString(bytes.AsSpan(start));
    }

    private void TrimToLimit(int limit)
    {
        while (_retainedBytes > limit)
        {
            var overflow = _retainedBytes - limit;
            var oldest = _chunks.First!.Value;
            _chunks.RemoveFirst();
            if (oldest.Bytes.Length <= overflow)
            {
                _retainedBytes -= oldest.Bytes.Length;
                continue;
            }

            var start = overflow;
            while (start < oldest.Bytes.Length && IsUtf8ContinuationByte(oldest.Bytes[start]))
            {
                start++;
            }

            var tail = oldest.Bytes[start..];
            _chunks.AddFirst(new OutputChunk(oldest.Stream, tail));
            _retainedBytes -= start;
        }
    }

    private static bool IsUtf8ContinuationByte(byte value) => (value & 0xC0) == 0x80;

    private static int GetMaximumMatchByteCount(string secret, bool caseInsensitive)
    {
        if (!caseInsensitive)
        {
            return Utf8.GetByteCount(secret);
        }

        var maximumBytes = 0L;
        for (var index = 0; index < secret.Length; index++)
        {
            if (char.IsHighSurrogate(secret[index])
                && index + 1 < secret.Length
                && char.IsLowSurrogate(secret[index + 1]))
            {
                maximumBytes += 4;
                index++;
            }
            else
            {
                maximumBytes += 3;
            }
        }

        return maximumBytes > int.MaxValue ? int.MaxValue : (int) maximumBytes;
    }

    private readonly record struct OutputChunk(ModuleOutputStream Stream, byte[] Bytes);
}

internal enum ModuleOutputStream
{
    StandardOutput,
    StandardError,
}
