namespace ModularPipelines;

internal class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string value) => Console.WriteLine(value);
}