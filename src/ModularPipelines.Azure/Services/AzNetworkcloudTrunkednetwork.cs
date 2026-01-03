using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudTrunkednetworkDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzNetworkcloudTrunkednetworkListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudTrunkednetworkShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudTrunkednetworkUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudTrunkednetworkWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudTrunkednetworkWaitOptions(), cancellationToken: token);
    }
}