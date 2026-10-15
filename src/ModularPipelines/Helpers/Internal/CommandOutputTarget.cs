using System.Text;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Receives one output stream of a command: captures the raw text and passes each complete line to a logger.
/// Lines end at <c>\n</c>, <c>\r</c> or <c>\r\n</c>, as <see cref="TextReader.ReadLine"/> splits them.
/// Logged lines retain at most 65,536 characters, followed by a truncation marker when longer.
/// </summary>
internal sealed class CommandOutputTarget(
    BoundedCommandOutputBuffer buffer,
    BoundedCommandOutputBuffer? completeOutputBuffer,
    Action<string>? logLine)
{
    private const int MaximumLogLineLength = 64 * 1024;
    private readonly StringBuilder _line = new();
    private bool _skipLineFeed;
    private bool _discardLine;

    public void Append(ReadOnlySpan<char> characters)
    {
        buffer.Append(characters);
        completeOutputBuffer?.Append(characters);

        if (logLine is null)
        {
            return;
        }

        foreach (var character in characters)
        {
            if (_skipLineFeed)
            {
                _skipLineFeed = false;
                if (character == '\n')
                {
                    continue;
                }
            }

            if (character is '\r' or '\n')
            {
                _skipLineFeed = character == '\r';
                if (!_discardLine)
                {
                    FlushLine();
                }

                _discardLine = false;
                continue;
            }

            if (_discardLine)
            {
                continue;
            }

            if (_line.Length == MaximumLogLineLength)
            {
                _discardLine = true;
                FlushLine(truncated: true);
                continue;
            }

            _line.Append(character);
        }
    }

    public void Complete()
    {
        if (_line.Length > 0)
        {
            FlushLine();
        }
    }

    private void FlushLine(bool truncated = false)
    {
        var line = _line.ToString();
        _line.Clear();
        logLine!(truncated ? line + " ... [line truncated]" : line);
    }
}
