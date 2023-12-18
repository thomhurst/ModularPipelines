using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudCloudservicesnetwork
{
    public AzNetworkcloudCloudservicesnetwork(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudCloudservicesnetworkCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudCloudservicesnetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudCloudservicesnetworkDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudCloudservicesnetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudCloudservicesnetworkListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudCloudservicesnetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudCloudservicesnetworkShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudCloudservicesnetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudCloudservicesnetworkUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudCloudservicesnetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudCloudservicesnetworkWaitOptions(), token);
    }
}

