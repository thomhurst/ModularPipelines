using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder")]
public class AzEdgeorderOrderItem
{
    public AzEdgeorderOrderItem(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Cancel(AzEdgeorderOrderItemCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzEdgeorderOrderItemCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzEdgeorderOrderItemDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderOrderItemDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzEdgeorderOrderItemListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderOrderItemListOptions(), token);
    }

    public async Task<CommandResult> Return(AzEdgeorderOrderItemReturnOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzEdgeorderOrderItemShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderOrderItemShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzEdgeorderOrderItemUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzEdgeorderOrderItemUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzEdgeorderOrderItemWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}