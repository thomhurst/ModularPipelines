using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "deployment")]
public class AzWebappDeploymentSource
{
    public AzWebappDeploymentSource(
        AzWebappDeploymentSourceConfigZip configZip,
        ICommand internalCommand
    )
    {
        ConfigZipCommands = configZip;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzWebappDeploymentSourceConfigZip ConfigZipCommands { get; }

    public async Task<CommandResult> Config(AzWebappDeploymentSourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigLocalGit(AzWebappDeploymentSourceConfigLocalGitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigZip(AzWebappDeploymentSourceConfigZipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzWebappDeploymentSourceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentSourceDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzWebappDeploymentSourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentSourceShowOptions(), token);
    }

    public async Task<CommandResult> Sync(AzWebappDeploymentSourceSyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentSourceSyncOptions(), token);
    }

    public async Task<CommandResult> UpdateToken(AzWebappDeploymentSourceUpdateTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzWebappDeploymentSourceUpdateTokenOptions(), token);
    }
}