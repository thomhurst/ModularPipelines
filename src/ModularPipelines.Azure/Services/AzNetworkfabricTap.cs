using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricTap
{
    public AzNetworkfabricTap(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricTapCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricTapDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricTapListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricTapShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricTapUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapUpdateOptions(), token);
    }

    public async Task<CommandResult> UpdateAdminState(AzNetworkfabricTapUpdateAdminStateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapUpdateAdminStateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricTapWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricTapWaitOptions(), token);
    }
}