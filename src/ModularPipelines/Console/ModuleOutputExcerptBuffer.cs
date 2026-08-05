using System.Text;
using ModularPipelines.Models;

namespace ModularPipelines.Console;

internal sealed class ModuleOutputExcerptBuffer(int maximumBytes)
{
    private static readonly Encoding Utf8 = new UTF8Encoding(
        encoderShouldEmitUTF8Identifier: false,
        throwOnInvalidBytes: false);
    private static readonly byte[] NewLineBytes = Utf8.GetBytes(Environment.NewLine);
    private readonly LinkedList<OutputChunk> _chunks = [];
    private long _totalBytes;
    private int _retainedBytes;

    public void Append(string value, ModuleOutputStream stream)
    {
        _totalBytes += (long)Utf8.GetByteCount(value) + NewLineBytes.Length;

        // One UTF-16 code unit always produces at least one UTF-8 byte. Starting no more than
        // maximumBytes code units from the end bounds the temporary allocation as well as storage.
        var start = Math.Max(0, value.Length - maximumBytes);
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
        TrimToLimit();
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

        return new ModuleOutputExcerpt
        {
            StdoutTail = stdout.Length == 0 ? null : stdout.ToString(),
            StderrTail = stderr.Length == 0 ? null : stderr.ToString(),
            TruncatedBytes = _totalBytes - _retainedBytes,
        };
    }

    private void TrimToLimit()
    {
        while (_retainedBytes > maximumBytes)
        {
            var overflow = _retainedBytes - maximumBytes;
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

    private readonly record struct OutputChunk(ModuleOutputStream Stream, byte[] Bytes);
}

internal enum ModuleOutputStream
{
    StandardOutput,
    StandardError,
}
