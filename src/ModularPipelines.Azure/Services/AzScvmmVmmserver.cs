using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scvmm")]
public class AzScvmmVmmserver
{
    public AzScvmmVmmserver(
        AzScvmmVmmserverInventoryItem inventoryItem,
        ICommand internalCommand
    )
    {
        InventoryItem = inventoryItem;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzScvmmVmmserverInventoryItem InventoryItem { get; }

    public async Task<CommandResult> Connect(AzScvmmVmmserverConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzScvmmVmmserverDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzScvmmVmmserverListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverListOptions(), token);
    }

    public async Task<CommandResult> Show(AzScvmmVmmserverShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzScvmmVmmserverUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmmserverWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}