using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricL3domain
{
    public AzNetworkfabricL3domain(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricL3domainCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricL3domainDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricL3domainListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricL3domainShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricL3domainUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateAdminState(AzNetworkfabricL3domainUpdateAdminStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainUpdateAdminStateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricL3domainWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL3domainWaitOptions(), token);
    }
}