using ModularPipelines.Context.Domains.Shell;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides access to command execution capabilities including CLI tools, Bash, and PowerShell.
/// </summary>
public interface IShellContext
{
    /// <summary>
    /// Execute general CLI commands.
    /// </summary>
    ICommandContext Command { get; }

    /// <summary>
    /// Execute Bash scripts and commands.
    /// </summary>
    IBashContext Bash { get; }

    /// <summary>
    /// Execute PowerShell scripts and commands.
    /// </summary>
    IPowerShellContext PowerShell { get; }
}
