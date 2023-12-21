using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudL2network
{
    public AzNetworkcloudL2network(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudL2networkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudL2networkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL2networkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudL2networkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL2networkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudL2networkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL2networkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudL2networkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL2networkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudL2networkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudL2networkWaitOptions(), token);
    }
}