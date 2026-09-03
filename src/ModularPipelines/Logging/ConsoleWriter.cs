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
    private readonly ISecretProvider _secretProvider;
    private readonly IAnsiConsole _ansiConsole;

    public ConsoleWriter(
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        IAnsiConsole ansiConsole)
    {
        _secretObfuscator = secretObfuscator;
        _secretProvider = secretProvider;
        _ansiConsole = ansiConsole;
    }

    public void WriteLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteLine(value);
            return;
        }

        ExecuteWithStableSecrets(value, WriteLineCore);
    }

    public void WriteMarkupLine(string value)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.WriteMarkupLine(value);
            return;
        }

        ExecuteWithStableSecrets(value, WriteMarkupLineCore);
    }

    public void Write(IRenderable renderable)
    {
        if (TryGetModuleConsoleWriter(out var moduleConsoleWriter))
        {
            moduleConsoleWriter.Write(renderable);
            return;
        }

        ExecuteWithStableSecrets(renderable, WriteCore);
    }

    private void WriteLineCore(string value) =>
        _ansiConsole.WriteLine(_secretObfuscator.Obfuscate(value, null));

    private void WriteMarkupLineCore(string value)
    {
        try
        {
            _ansiConsole.Write(ObfuscatedMarkup.Create(value, _secretObfuscator));
            _ansiConsole.WriteLine();
        }
        catch (InvalidOperationException)
        {
            // Fall back to plain console output if markup parsing fails
            // (e.g., unbalanced or invalid markup characters)
            WriteLineCore(value);
        }
    }

    private void WriteCore(IRenderable renderable) =>
        _ansiConsole.Write(new SecretObfuscatedRenderable(renderable, _secretObfuscator));

    private void ExecuteWithStableSecrets<TState>(TState state, Action<TState> write)
    {
        if (_secretProvider is ISecretEmissionGuard emissionGuard)
        {
            emissionGuard.ExecuteWithStableSecrets(state, write);
            return;
        }

        write(state);
    }

    private static bool TryGetModuleConsoleWriter(
        [NotNullWhen(true)] out IConsoleWriter? consoleWriter)
    {
        consoleWriter = ModuleLogger.Values.Value as IConsoleWriter;
        return consoleWriter is not null;
    }
}
