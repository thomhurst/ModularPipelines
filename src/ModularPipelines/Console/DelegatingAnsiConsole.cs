using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.Console;

/// <summary>
/// A delegating wrapper that always forwards to the current AnsiConsole.Console.
/// This allows the console instance to be replaced (e.g., by ConsoleCoordinator)
/// while loggers continue to use the correct console.
/// </summary>
internal sealed class DelegatingAnsiConsole : IAnsiConsole
{
    public static DelegatingAnsiConsole Instance { get; } = new();

    private DelegatingAnsiConsole()
    {
    }

    private static IAnsiConsole Console => AnsiConsole.Console;

    public Profile Profile => Console.Profile;

    public IAnsiConsoleCursor Cursor => Console.Cursor;

    public IAnsiConsoleInput Input => Console.Input;

    public IExclusivityMode ExclusivityMode => Console.ExclusivityMode;

    public RenderPipeline Pipeline => Console.Pipeline;

    public void Clear(bool home) => Console.Clear(home);

    public void Write(IRenderable renderable) => Console.Write(renderable);
}
