using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Options.Windows.Chocolatey;

namespace ModularPipelines.Context.Windows;

/// <summary>
/// Implementation of Chocolatey package manager operations for Windows.
/// </summary>
[ExcludeFromCodeCoverage]
public class Chocolatey : IChocolatey
{
    private readonly ICommand _command;

    /// <summary>
    /// Initializes a new instance of the <see cref="Chocolatey"/> class.
    /// </summary>
    /// <param name="command">The command execution service.</param>
    public Chocolatey(ICommand command)
    {
        _command = command;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Install(ChocolateyInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Uninstall(ChocolateyUninstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Upgrade(ChocolateyUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> List(ChocolateyListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ChocolateyListOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Search(ChocolateySearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Info(ChocolateyInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Outdated(ChocolateyOutdatedOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new ChocolateyOutdatedOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Pin(ChocolateyPinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Custom(ChocolateyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }
}
