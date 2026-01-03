using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("scvmm")]
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
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzScvmmVmmserverDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzScvmmVmmserverListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzScvmmVmmserverShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzScvmmVmmserverUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzScvmmVmmserverUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzScvmmVmmserverWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}