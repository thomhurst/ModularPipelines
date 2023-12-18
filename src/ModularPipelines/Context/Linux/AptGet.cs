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
    public async Task<CommandResult> Autoclean(AptGetAutocleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetAutocleanOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetBuildDepOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCheckOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCleanOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetDistUpgradeOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetPackageOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetSourceOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpdateOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpgradeOptions(), token);
    }

    /// <inheritdoc/>
    public async Task<CommandResult> Custom(AptGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}