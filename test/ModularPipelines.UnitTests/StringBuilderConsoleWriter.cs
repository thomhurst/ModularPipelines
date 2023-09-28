using System.Text;

namespace ModularPipelines.UnitTests;

public class StringBuilderConsoleWriter : IConsoleWriter
{
    private readonly StringBuilder _stringBuilder;

    public StringBuilderConsoleWriter(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public void LogToConsole(string value)
    {
        _stringBuilder.AppendLine(value);
    }
}