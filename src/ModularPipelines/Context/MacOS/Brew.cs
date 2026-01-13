using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Options.MacOS.Brew;

namespace ModularPipelines.Context.MacOS;

/// <summary>
/// Implementation of Homebrew package manager operations for macOS.
/// </summary>
[ExcludeFromCodeCoverage]
public class Brew : IBrew
{
    private readonly ICommand _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="Brew"/> class.
    /// </summary>
    /// <param name="command">The command execution service.</param>
    public Brew(ICommand command)
    {
        _command = command;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Install(BrewInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Uninstall(BrewUninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Update(BrewUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new BrewUpdateOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Upgrade(BrewUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new BrewUpgradeOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> List(BrewListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new BrewListOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Search(BrewSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Info(BrewInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Tap(BrewTapOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Untap(BrewUntapOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Cleanup(BrewCleanupOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new BrewCleanupOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Custom(BrewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }
}
