using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Engine;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Logging;

[ExcludeFromCodeCoverage]
internal class ConsoleWriter : IConsoleWriter
{
    private readonly ISecretObfuscator _secretObfuscator;

    public ConsoleWriter(ISecretObfuscator secretObfuscator)
    {
        _secretObfuscator = secretObfuscator;
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

        try
        {
            AnsiConsole.Write(ObfuscatedMarkup.Create(value, _secretObfuscator));
            AnsiConsole.WriteLine();
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            AnsiConsole.WriteLine(_secretObfuscator.Obfuscate(value, null));
        }
    }

    public void Write(IRenderable renderable)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.Write(renderable);
            return;
        }

        AnsiConsole.Write(new SecretObfuscatedRenderable(renderable, _secretObfuscator));
    }

    private static bool TryGetModuleConsoleWriter(
        [NotNullWhen(true)] out IConsoleWriter? consoleWriter)
    {
        consoleWriter = ModuleLogger.Values.Value as IConsoleWriter;
        return consoleWriter is not null;
    }
}
