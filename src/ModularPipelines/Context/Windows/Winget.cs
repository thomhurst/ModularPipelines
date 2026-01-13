using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Options.Windows.Winget;

namespace ModularPipelines.Context.Windows;

/// <summary>
/// Implementation of Windows Package Manager (winget) operations.
/// </summary>
[ExcludeFromCodeCoverage]
public class Winget : IWinget
{
    private readonly ICommand _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="Winget"/> class.
    /// </summary>
    /// <param name="command">The command execution service.</param>
    public Winget(ICommand command)
    {
        _command = command;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Install(WingetInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Uninstall(WingetUninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Upgrade(WingetUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new WingetUpgradeOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> List(WingetListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new WingetListOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Search(WingetSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Show(WingetShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Export(WingetExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Import(WingetImportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Source(WingetSourceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Custom(WingetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }
}
