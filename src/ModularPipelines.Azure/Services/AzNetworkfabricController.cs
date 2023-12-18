using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkfabric")]
public class AzNetworkfabricController
{
    public AzNetworkfabricController(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkfabricControllerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkfabricControllerDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricControllerDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkfabricControllerListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricControllerListOptions(), token);
    }

    public async Task<CommandResult> Show(AzNetworkfabricControllerShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricControllerShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkfabricControllerUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricControllerUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkfabricControllerWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkfabricControllerWaitOptions(), token);
    }
}