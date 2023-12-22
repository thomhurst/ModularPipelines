using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricL2domain
{
    public AzNetworkfabricL2domain(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricL2domainCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricL2domainDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricL2domainListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricL2domainShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricL2domainUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateAdminState(AzNetworkfabricL2domainUpdateAdminStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainUpdateAdminStateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricL2domainWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricL2domainWaitOptions(), token);
    }
}