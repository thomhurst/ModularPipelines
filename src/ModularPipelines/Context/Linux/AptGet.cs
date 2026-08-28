using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

[ExcludeFromCodeCoverage]
internal class AptGet : IAptGet
{
    private readonly ICommandContext _command;

    public AptGet(ICommandContext command)
    {
        _command = command;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> AutocleanAsync(AptGetAutocleanOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetAutocleanOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> BuildDepAsync(AptGetBuildDepOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetBuildDepOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CheckAsync(AptGetCheckOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetCheckOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CleanAsync(AptGetCleanOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetCleanOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> DistUpgradeAsync(AptGetDistUpgradeOptions? options = default,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetDistUpgradeOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> InstallAsync(AptGetInstallOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> PackageAsync(AptGetPackageOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetPackageOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> RemoveAsync(AptGetRemoveOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> SourceAsync(AptGetSourceOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetSourceOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpdateAsync(AptGetUpdateOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetUpdateOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> UpgradeAsync(AptGetUpgradeOptions? options = default, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options ?? new AptGetUpgradeOptions(), null, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> CustomAsync(AptGetOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options, null, cancellationToken).ConfigureAwait(false);
    }
}
