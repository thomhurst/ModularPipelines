using ModularPipelines.Models;
using ModularPipelines.Options.Linux.AptGet;

namespace ModularPipelines.Context.Linux;

public class AptGet : IAptGet
{
    private readonly ICommand _command;

    public AptGet(ICommand command)
    {
        _command = command;
    }
    
    public async Task<CommandResult> Autoclean(AptGetAutocleanOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetAutocleanOptions(), token);
    }

    public async Task<CommandResult> BuildDep(AptGetBuildDepOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetBuildDepOptions(), token);
    }

    public async Task<CommandResult> Check(AptGetCheckOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCheckOptions(), token);
    }

    public async Task<CommandResult> Clean(AptGetCleanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetCleanOptions(), token);
    }

    public async Task<CommandResult> DistUpgrade(AptGetDistUpgradeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetDistUpgradeOptions(), token);
    }

    public async Task<CommandResult> Install(AptGetInstallOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Package(AptGetPackageOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetPackageOptions(), token);
    }

    public async Task<CommandResult> Remove(AptGetRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Source(AptGetSourceOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetSourceOptions(), token);
    }

    public async Task<CommandResult> Update(AptGetUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpdateOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AptGetUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AptGetUpgradeOptions(), token);
    }

    public async Task<CommandResult> Custom(AptGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}