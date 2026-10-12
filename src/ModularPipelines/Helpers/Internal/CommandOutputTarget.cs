using System.Text;

namespace ModularPipelines.Helpers.Internal;

/// <summary>
/// Receives one output stream of a command: captures the raw text and passes each complete line to a logger.
/// Lines end at <c>\n</c>, <c>\r</c> or <c>\r\n</c>, as <see cref="TextReader.ReadLine"/> splits them.
/// </summary>
internal sealed class CommandOutputTarget(
    BoundedCommandOutputBuffer buffer,
    BoundedCommandOutputBuffer? completeOutputBuffer,
    Action<string>? logLine)
{
    private readonly StringBuilder _line = new();
    private bool _skipLineFeed;

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
                FlushLine();
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

    private void FlushLine()
    {
        var line = _line.ToString();
        _line.Clear();
        logLine!(line);
    }
}
