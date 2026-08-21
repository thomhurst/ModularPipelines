using System.Text;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Models;

namespace ModularPipelines.Console;

internal sealed class ModuleOutputExcerptBuffer(
    int maximumBytes,
    ISecretObfuscator? secretObfuscator = null,
    ISecretProvider? secretProvider = null,
    ILogger? logger = null)
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

    public void Append(
        string value,
        ModuleOutputStream stream,
        bool appendNewLine = true)
    {
        var lineTerminatorByteCount = appendNewLine ? NewLineBytes.Length : 0;
        var appendedBytes = (long) Utf8.GetByteCount(value) + lineTerminatorByteCount;
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
        var bytes = new byte[valueByteCount + lineTerminatorByteCount];
        Utf8.GetBytes(retainedValue, bytes);
        if (appendNewLine)
        {
            NewLineBytes.CopyTo(bytes, valueByteCount);
        }
        _chunks.AddLast(new OutputChunk(stream, bytes, Utf8.GetString(bytes)));
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
                .Append(chunk.Text);
        }

        var stdoutValue = stdout.ToString();
        var stderrValue = stderr.ToString();
        var (stdoutBytes, stderrBytes) = GetFinalStreamByteLimits(stdoutValue, stderrValue);
        if (!TryCreateTails(
                stdoutValue,
                stderrValue,
                stdoutBytes,
                stderrBytes,
                out var stdoutTail,
                out var stderrTail,
                out var retainedSourceBytes))
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
            TruncatedBytes = Math.Max(0, _totalBytes - retainedSourceBytes),
            SecretPatternsVersion = secretPatternsVersion,
        };
    }

    private bool TryCreateTails(
        string stdout,
        string stderr,
        int stdoutBytes,
        int stderrBytes,
        out string? stdoutTail,
        out string? stderrTail,
        out long retainedSourceBytes)
    {
        if (secretObfuscator is null && secretProvider is null)
        {
            stdoutTail = GetUtf8Tail(stdout, stdoutBytes);
            stderrTail = GetUtf8Tail(stderr, stderrBytes);
            retainedSourceBytes = Utf8.GetByteCount(stdoutTail ?? string.Empty)
                                  + Utf8.GetByteCount(stderrTail ?? string.Empty);
            return true;
        }

        return TryCreateMaskedTails(
            stdout,
            stderr,
            out stdoutTail,
            out stderrTail,
            out retainedSourceBytes);
    }

    private bool TryCreateMaskedTails(
        string stdout,
        string stderr,
        out string? stdoutTail,
        out string? stderrTail,
        out long retainedSourceBytes)
    {
        stdoutTail = null;
        stderrTail = null;
        retainedSourceBytes = 0;

        // Custom or incomplete masking dependencies do not expose enough information
        // to prove that a bounded context is safe.
        if (secretObfuscator is not SecretObfuscator concreteObfuscator || secretProvider is null)
        {
            logger?.LogDebug(
                "Omitting module output excerpt because its masking dependencies cannot provide a safe source map.");
            return false;
        }

        var caseInsensitive = concreteObfuscator.CaseInsensitive;
        var snapshot = secretProvider.GetSnapshot();
        if (!concreteObfuscator.CanSafelyPreserveMasks(snapshot.Secrets))
        {
            logger?.LogDebug(
                "Omitting module output excerpt because the configured mask contains a registered secret.");
            return false;
        }

        var maximumMatchBytes = snapshot.Secrets
            .Where(static secret => !string.IsNullOrEmpty(secret))
            .Select(secret => GetMaximumMatchByteCount(secret, caseInsensitive))
            .DefaultIfEmpty()
            .Max();
        if (maximumMatchBytes > maximumBytes)
        {
            logger?.LogDebug(
                "Omitting module output excerpt because a possible secret match of {MaximumMatchBytes} UTF-8 bytes exceeds the {MaximumExcerptBytes}-byte excerpt cap.",
                maximumMatchBytes,
                maximumBytes);
            return false;
        }

        var maskedStdout = concreteObfuscator.ObfuscatePreservingMasksWithSourceMap(stdout);
        var maskedStderr = concreteObfuscator.ObfuscatePreservingMasksWithSourceMap(stderr);
        var (stdoutBytes, stderrBytes) = RebalanceMaskedStreamByteLimits(
            maskedStdout,
            maskedStderr);

        if (!TryGetSafeMaskedTail(
                maskedStdout.Value,
                stdoutBytes,
                _totalStdoutBytes > Utf8.GetByteCount(stdout),
                maximumMatchBytes,
                out stdoutTail)
            || !TryGetSafeMaskedTail(
                maskedStderr.Value,
                stderrBytes,
                _totalStderrBytes > Utf8.GetByteCount(stderr),
                maximumMatchBytes,
                out stderrTail))
        {
            stdoutTail = stderrTail = null;
            logger?.LogDebug(
                "Omitting module output excerpt because the retained context cannot prove both masked stream boundaries safe.");
            return false;
        }

        retainedSourceBytes = GetRetainedSourceByteCount(maskedStdout, stdout, stdoutTail)
                              + GetRetainedSourceByteCount(maskedStderr, stderr, stderrTail);

        // If registration changed during masking, a new secret may cross the retained
        // boundary. Omit the excerpt rather than risk returning a partial secret.
        if (secretProvider.Version != snapshot.Version
            || concreteObfuscator.CaseInsensitive != caseInsensitive)
        {
            retainedSourceBytes = 0;
            logger?.LogDebug(
                "Omitting module output excerpt because secret masking configuration changed while the excerpt was created.");
            return false;
        }

        return true;
    }

    private static int GetRetainedSourceByteCount(
        SecretObfuscator.MappedObfuscatedOutput maskedOutput,
        string source,
        string? tail)
    {
        var tailBytes = Utf8.GetByteCount(tail ?? string.Empty);
        var sourceOffset = maskedOutput.GetSourceOffsetForOutputSuffix(tailBytes);
        return Utf8.GetByteCount(source.AsSpan(sourceOffset));
    }

    private (int StdoutBytes, int StderrBytes) RebalanceMaskedStreamByteLimits(
        SecretObfuscator.MappedObfuscatedOutput maskedStdout,
        SecretObfuscator.MappedObfuscatedOutput maskedStderr)
    {
        var maskedStdoutBytes = Utf8.GetBytes(maskedStdout.Value);
        var maskedStderrBytes = Utf8.GetBytes(maskedStderr.Value);
        var stdoutBytes = 0;
        var stderrBytes = 0;
        var stdoutOffset = 0;
        var stderrOffset = 0;
        var chunks = new List<MappedOutputChunk>(_chunks.Count);
        foreach (var chunk in _chunks)
        {
            var textLength = chunk.Text.Length;
            if (chunk.Stream is ModuleOutputStream.StandardError)
            {
                chunks.Add(new MappedOutputChunk(chunk.Stream, stderrOffset));
                stderrOffset += textLength;
            }
            else
            {
                chunks.Add(new MappedOutputChunk(chunk.Stream, stdoutOffset));
                stdoutOffset += textLength;
            }
        }

        for (var index = chunks.Count - 1; index >= 0; index--)
        {
            var chunk = chunks[index];
            if (chunk.Stream is ModuleOutputStream.StandardError)
            {
                stderrBytes = RebalanceStreamBytes(
                    maskedStderr.GetSuffixByteCount(chunk.SourceOffset),
                    stderrBytes,
                    stdoutBytes);
                stderrBytes = GetUtf8TailByteCount(maskedStderrBytes, stderrBytes);
            }
            else
            {
                stdoutBytes = RebalanceStreamBytes(
                    maskedStdout.GetSuffixByteCount(chunk.SourceOffset),
                    stdoutBytes,
                    stderrBytes);
                stdoutBytes = GetUtf8TailByteCount(maskedStdoutBytes, stdoutBytes);
            }
        }

        return (stdoutBytes, stderrBytes);
    }

    private int RebalanceStreamBytes(
        int availableBytes,
        int allocatedBytes,
        int otherStreamBytes)
    {
        allocatedBytes = Math.Min(allocatedBytes, availableBytes);
        var remaining = Math.Max(0, maximumBytes - allocatedBytes - otherStreamBytes);
        return allocatedBytes + Math.Min(availableBytes - allocatedBytes, remaining);
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

    private (int StdoutBytes, int StderrBytes) GetFinalStreamByteLimits(
        string stdout,
        string stderr)
    {
        var stdoutUtf8 = Utf8.GetBytes(stdout);
        var stderrUtf8 = Utf8.GetBytes(stderr);
        var stdoutBytes = 0;
        var stderrBytes = 0;
        for (var chunk = _chunks.Last;
             chunk is not null && stdoutBytes + stderrBytes < maximumBytes;
             chunk = chunk.Previous)
        {
            var remaining = maximumBytes - stdoutBytes - stderrBytes;
            var retained = Math.Min(chunk.Value.Bytes.Length, remaining);
            if (chunk.Value.Stream is ModuleOutputStream.StandardError)
            {
                stderrBytes += retained;
                stderrBytes = GetUtf8TailByteCount(stderrUtf8, stderrBytes);
            }
            else
            {
                stdoutBytes += retained;
                stdoutBytes = GetUtf8TailByteCount(stdoutUtf8, stdoutBytes);
            }
        }

        return (stdoutBytes, stderrBytes);
    }

    private static int GetUtf8TailByteCount(byte[] bytes, int maximumTailBytes)
    {
        if (maximumTailBytes == 0 || bytes.Length == 0)
        {
            return 0;
        }

        if (bytes.Length <= maximumTailBytes)
        {
            return bytes.Length;
        }

        var start = GetUtf8Boundary(bytes, bytes.Length - maximumTailBytes);

        return bytes.Length - start;
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

        var start = GetUtf8Boundary(bytes, bytes.Length - maximumTailBytes);

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

            var start = GetUtf8Boundary(oldest.Bytes, overflow);

            var tail = oldest.Bytes[start..];
            _chunks.AddFirst(new OutputChunk(oldest.Stream, tail, Utf8.GetString(tail)));
            _retainedBytes -= start;
        }
    }

    private static int GetUtf8Boundary(byte[] bytes, int start)
    {
        while (start < bytes.Length && IsUtf8ContinuationByte(bytes[start]))
        {
            start++;
        }

        return start;
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

    private readonly record struct OutputChunk(
        ModuleOutputStream Stream,
        byte[] Bytes,
        string Text);

    private readonly record struct MappedOutputChunk(ModuleOutputStream Stream, int SourceOffset);
}

internal enum ModuleOutputStream
{
    StandardOutput,
    StandardError,
}
