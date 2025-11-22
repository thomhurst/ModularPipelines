using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    public void LogToConsole(string value) => Console.WriteLine(value);
}
