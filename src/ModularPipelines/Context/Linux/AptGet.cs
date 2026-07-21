using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

[ExcludeFromCodeCoverage]
public class AptGet : IAptGet
{
    private readonly ICommand _command;

    public AptGet(ICommand command)
    {
        _command = command;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Autoclean(AptGetAutocleanOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetAutocleanOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetBuildDepOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCheckOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCleanOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetDistUpgradeOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetPackageOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetSourceOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpdateOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpgradeOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Custom(AptGetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, cancellationToken).ConfigureAwait(false);
    }
}