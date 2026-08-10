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
    private int _retainedBytes;

    public void Append(string value, ModuleOutputStream stream)
    {
        _totalBytes += (long) Utf8.GetByteCount(value) + NewLineBytes.Length;

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

        var stdout = new StringBuilder();
        var stderr = new StringBuilder();
        foreach (var chunk in _chunks)
        {
            (chunk.Stream is ModuleOutputStream.StandardError ? stderr : stdout)
                .Append(Utf8.GetString(chunk.Bytes));
        }

        var (stdoutBytes, stderrBytes) = GetFinalStreamByteLimits();
        if (!TryCreateTails(
                stdout.ToString(),
                stderr.ToString(),
                stdoutBytes,
                stderrBytes,
                out var stdoutTail,
                out var stderrTail))
        {
            return null;
        }

        return new ModuleOutputExcerpt
        {
            StdoutTail = stdoutTail,
            StderrTail = stderrTail,
            TruncatedBytes = Math.Max(0, _totalBytes - maximumBytes),
        };
    }

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

        // Custom or incomplete masking dependencies do not expose enough information
        // to prove that a bounded context is safe.
        if (secretObfuscator is not SecretObfuscator concreteObfuscator || secretProvider is null)
        {
            stdoutTail = null;
            stderrTail = null;
            return false;
        }

        var caseInsensitive = concreteObfuscator.CaseInsensitive;
        var snapshot = secretProvider.GetSnapshot();
        if (snapshot.Secrets
            .Where(static secret => !string.IsNullOrEmpty(secret))
            .Any(secret => GetMaximumMatchByteCount(secret, caseInsensitive) > maximumBytes))
        {
            stdoutTail = null;
            stderrTail = null;
            return false;
        }

        stdoutTail = GetUtf8Tail(secretObfuscator.Obfuscate(stdout, null), stdoutBytes);
        stderrTail = GetUtf8Tail(secretObfuscator.Obfuscate(stderr, null), stderrBytes);

        // If registration changed during masking, a new secret may cross the retained
        // boundary. Omit the excerpt rather than risk returning a partial secret.
        return secretProvider.Version == snapshot.Version
               && concreteObfuscator.CaseInsensitive == caseInsensitive;
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

    private static string? GetUtf8Tail(string value, int maximumTailBytes)
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
