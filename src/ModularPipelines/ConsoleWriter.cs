using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string value) => Console.WriteLine(value);
}