using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-endpoint")]
public class AzNetworkPrivateEndpointDnsZoneGroup
{
    public AzNetworkPrivateEndpointDnsZoneGroup(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Add(AzNetworkPrivateEndpointDnsZoneGroupAddOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzNetworkPrivateEndpointDnsZoneGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkPrivateEndpointDnsZoneGroupDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointDnsZoneGroupDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkPrivateEndpointDnsZoneGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Remove(AzNetworkPrivateEndpointDnsZoneGroupRemoveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkPrivateEndpointDnsZoneGroupShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointDnsZoneGroupShowOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkPrivateEndpointDnsZoneGroupWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkPrivateEndpointDnsZoneGroupWaitOptions(), token);
    }
}