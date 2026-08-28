using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to command execution capabilities including CLI tools, Bash, and PowerShell.
/// </summary>
internal class ShellContext : IShellContext
{
    private readonly ICommandContext _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellContext"/> class.
    /// </summary>
    /// <param name="command">The command context for CLI execution.</param>
    /// <param name="bash">The bash context for bash script execution.</param>
    /// <param name="powerShell">The PowerShell context for PowerShell script execution.</param>
    public ShellContext(ICommandContext command, IBashContext bash, IPowerShellContext powerShell)
    {
        _command = command;
        Bash = bash;
        PowerShell = powerShell;
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunAsync(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunAsync(
        string tool,
        IReadOnlyList<string> arguments,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(
            new CommandLineToolOptions(tool) { Arguments = arguments },
            executionOptions,
            cancellationToken);
    }

    /// <inheritdoc />
    public virtual Task<CommandResult> RunAsync(
        string tool,
        IReadOnlyList<string> arguments,
        CancellationToken cancellationToken)
    {
        return RunAsync(tool, arguments, null, cancellationToken);
    }

    /// <inheritdoc />
    public IBashContext Bash { get; }

    /// <inheritdoc />
    public IPowerShellContext PowerShell { get; }
}
