using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment")]
public class AzNginxDeploymentConfiguration
{
    public AzNginxDeploymentConfiguration(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNginxDeploymentConfigurationCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNginxDeploymentConfigurationDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentConfigurationDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNginxDeploymentConfigurationListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNginxDeploymentConfigurationShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentConfigurationShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNginxDeploymentConfigurationUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentConfigurationUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNginxDeploymentConfigurationWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNginxDeploymentConfigurationWaitOptions(), token);
    }
}