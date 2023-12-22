using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudTrunkednetwork
{
    public AzNetworkcloudTrunkednetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudTrunkednetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudTrunkednetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudTrunkednetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudTrunkednetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudTrunkednetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudTrunkednetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkWaitOptions(), token);
    }
}