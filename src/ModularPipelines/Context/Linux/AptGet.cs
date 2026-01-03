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
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetAutocleanOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetBuildDepOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCheckOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCleanOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetDistUpgradeOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetPackageOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetSourceOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpdateOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpgradeOptions(), null, token).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> Custom(AptGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, null, token).ConfigureAwait(false);
    }
}