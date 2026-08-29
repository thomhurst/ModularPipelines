using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Engine;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly object _renderLock = new();
    private readonly StringWriter _renderWriter;
    private readonly IAnsiConsole _renderConsole;

    public ConsoleWriter(ISecretObfuscator secretObfuscator)
    {
        _secretObfuscator = secretObfuscator;
        _renderWriter = new StringWriter();
        _renderConsole = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(_renderWriter),
            Ansi = AnsiSupport.No,
        });
    }

    public void WriteLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteLine(value);
            return;
        }

        AnsiConsole.WriteLine(_secretObfuscator.Obfuscate(value, null));
    }

    public void WriteMarkupLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteMarkupLine(value);
            return;
        }

        var obfuscated = _secretObfuscator.Obfuscate(value, null);

        try
        {
            AnsiConsole.MarkupLine(obfuscated);
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            System.Console.WriteLine(obfuscated);
        }
    }

    public void Write(IRenderable renderable)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.Write(renderable);
            return;
        }

        string rendered;
        lock (_renderLock)
        {
            _renderWriter.GetStringBuilder().Clear();
            _renderConsole.Write(renderable);
            rendered = _renderWriter.ToString();
        }

        AnsiConsole.WriteLine(_secretObfuscator.Obfuscate(rendered, null));
    }

    private static bool TryGetModuleConsoleWriter(
        [NotNullWhen(true)] out IConsoleWriter? consoleWriter)
    {
        consoleWriter = ModuleLogger.Values.Value as IConsoleWriter;
        return consoleWriter is not null;
    }
}
