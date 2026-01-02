using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to shell and command execution helpers.
/// </summary>
internal class ShellContext : IShellContext
{
    /// <inheritdoc />
    public IPowershell Powershell { get; }

    /// <inheritdoc />
    public IBash Bash { get; }

    /// <inheritdoc />
    public ICommand Command { get; }

    public ShellContext(IPowershell powershell, IBash bash, ICommand command)
    {
        Powershell = powershell;
        Bash = bash;
        Command = command;
    }
}
