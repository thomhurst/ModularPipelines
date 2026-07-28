using System.Text;

namespace ModularPipelines.Helpers.Internal;

internal sealed class BoundedCommandOutputBuffer
{
    private readonly int _maximumLength;
    private readonly int _headCapacity;
    private readonly int _tailCapacity;
    private readonly StringBuilder _head;
    private readonly StringBuilder? _unbounded;
    private char[]? _tail;
    private int _tailCount;
    private int _tailStart;
    private long _totalLength;

    public BoundedCommandOutputBuffer(int maximumLength)
    {
        _maximumLength = maximumLength;
        if (maximumLength <= 0)
        {
            _unbounded = new StringBuilder();
            _head = new StringBuilder();
            return;
        }

        _headCapacity = maximumLength / 2;
        _tailCapacity = maximumLength - _headCapacity;
        _head = new StringBuilder(Math.Min(_headCapacity, 256));
    }

    public override string ToString()
    {
        if (_unbounded is not null)
        {
            return _unbounded.ToString();
        }

        var result = new StringBuilder(_head.Length + _tailCount);
        result.Append(_head);

        if (_totalLength > _maximumLength)
        {
            result.AppendLine();
            result.Append("... [truncated ");
            result.Append(_totalLength - _maximumLength);
            result.AppendLine(" characters] ...");
        }

        AppendTailTo(result);
        return result.ToString();
    }

    public void Append(ReadOnlySpan<char> value)
    {
        _totalLength += value.Length;

        if (_unbounded is not null)
        {
            _unbounded.Append(value);
            return;
        }

        var remaining = value;
        var headRemaining = _headCapacity - _head.Length;
        if (headRemaining > 0)
        {
            var headLength = Math.Min(headRemaining, remaining.Length);
            _head.Append(remaining[..headLength]);
            remaining = remaining[headLength..];
        }

        AppendToTail(remaining);
    }

    private void AppendToTail(ReadOnlySpan<char> value)
    {
        if (_tailCapacity == 0 || value.IsEmpty)
        {
            return;
        }

        var tail = _tail ??= new char[_tailCapacity];
        if (value.Length >= tail.Length)
        {
            value[^tail.Length..].CopyTo(tail);
            _tailStart = 0;
            _tailCount = tail.Length;
            return;
        }

        var available = tail.Length - _tailCount;
        var initialLength = Math.Min(available, value.Length);
        WriteToTail(tail, (_tailStart + _tailCount) % tail.Length, value[..initialLength]);
        _tailCount += initialLength;
        value = value[initialLength..];

        if (value.IsEmpty)
        {
            return;
        }

        WriteToTail(tail, _tailStart, value);
        _tailStart = (_tailStart + value.Length) % tail.Length;
    }

    private static void WriteToTail(char[] tail, int index, ReadOnlySpan<char> value)
    {
        var firstLength = Math.Min(tail.Length - index, value.Length);
        value[..firstLength].CopyTo(tail.AsSpan(index));
        value[firstLength..].CopyTo(tail);
    }

    private void AppendTailTo(StringBuilder result)
    {
        if (_tailCount == 0 || _tail is null)
        {
            return;
        }

        var firstLength = Math.Min(_tail.Length - _tailStart, _tailCount);
        result.Append(_tail, _tailStart, firstLength);
        result.Append(_tail, 0, _tailCount - firstLength);
    }
}
