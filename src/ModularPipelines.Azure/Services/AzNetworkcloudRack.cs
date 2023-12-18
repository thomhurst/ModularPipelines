using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudRack
{
    public AzNetworkcloudRack(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> List(AzNetworkcloudRackListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudRackListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudRackShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudRackShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudRackUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudRackUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudRackWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudRackWaitOptions(), token);
    }
}