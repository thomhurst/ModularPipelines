using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Engine;
using ModularPipelines.Secrets;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly IAnsiConsole _ansiConsole;

    public ConsoleWriter(ISecretObfuscator secretObfuscator, IAnsiConsole ansiConsole)
    {
        _secretObfuscator = secretObfuscator;
        _ansiConsole = ansiConsole;
    }

    public void WriteLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteLine(value);
            return;
        }

        _ansiConsole.WriteLine(_secretObfuscator.Obfuscate(value, null));
    }

    public void WriteMarkupLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteMarkupLine(value);
            return;
        }

        try
        {
            _ansiConsole.Write(ObfuscatedMarkup.Create(value, _secretObfuscator));
            _ansiConsole.WriteLine();
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            _ansiConsole.WriteLine(_secretObfuscator.Obfuscate(value, null));
        }
    }

    public void Write(IRenderable renderable)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.Write(renderable);
            return;
        }

        _ansiConsole.Write(new SecretObfuscatedRenderable(renderable, _secretObfuscator));
    }

    private static bool TryGetModuleConsoleWriter(
        [NotNullWhen(true)] out IConsoleWriter? consoleWriter)
    {
        consoleWriter = ModuleLogger.Values.Value as IConsoleWriter;
        return consoleWriter is not null;
    }
}
