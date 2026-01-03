using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "deployment")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ConfigLocalGit(AzFunctionappDeploymentSourceConfigLocalGitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceConfigLocalGitOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> ConfigZip(AzFunctionappDeploymentSourceConfigZipOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzFunctionappDeploymentSourceDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzFunctionappDeploymentSourceShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Sync(AzFunctionappDeploymentSourceSyncOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceSyncOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> UpdateToken(AzFunctionappDeploymentSourceUpdateTokenOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzFunctionappDeploymentSourceUpdateTokenOptions(), cancellationToken: token);
    }
}