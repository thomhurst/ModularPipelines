using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "deployment")]
public class AzFunctionappDeploymentSource
{
    public AzFunctionappDeploymentSource(
        AzFunctionappDeploymentSourceConfigZip configZip,
        ICommand internalCommand
    )
    {
        ConfigZipCommands = configZip;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzFunctionappDeploymentSourceConfigZip ConfigZipCommands { get; }

    public async Task<CommandResult> Config(AzFunctionappDeploymentSourceConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigLocalGit(AzFunctionappDeploymentSourceConfigLocalGitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigZip(AzFunctionappDeploymentSourceConfigZipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzFunctionappDeploymentSourceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceDeleteOptions(), token);
    }

    public async Task<CommandResult> Show(AzFunctionappDeploymentSourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceShowOptions(), token);
    }

    public async Task<CommandResult> Sync(AzFunctionappDeploymentSourceSyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceSyncOptions(), token);
    }

    public async Task<CommandResult> UpdateToken(AzFunctionappDeploymentSourceUpdateTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceUpdateTokenOptions(), token);
    }
}