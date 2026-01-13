using ModularPipelines.Context.Domains.Shell;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to command execution capabilities including CLI tools, Bash, and PowerShell.
/// </summary>
internal class ShellContext : IShellContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellContext"/> class.
    /// </summary>
    /// <param name="command">The command context for CLI execution.</param>
    /// <param name="bash">The bash context for bash script execution.</param>
    /// <param name="powerShell">The PowerShell context for PowerShell script execution.</param>
    public ShellContext(ICommandContext command, IBashContext bash, IPowerShellContext powerShell)
    {
        Command = command;
        Bash = bash;
        PowerShell = powerShell;
    }

    /// <inheritdoc />
    public ICommandContext Command { get; }

    /// <inheritdoc />
    public IBashContext Bash { get; }

    /// <inheritdoc />
    public IPowerShellContext PowerShell { get; }
}
