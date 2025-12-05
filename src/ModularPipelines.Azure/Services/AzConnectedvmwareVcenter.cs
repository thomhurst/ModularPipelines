using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("connectedvmware")]
public class AzConnectedvmwareVcenter
{
    public AzConnectedvmwareVcenter(
        AzConnectedvmwareVcenterInventoryItem inventoryItem,
        ICommand internalCommand
    )
    {
        InventoryItem = inventoryItem;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzConnectedvmwareVcenterInventoryItem InventoryItem { get; }

    public async Task<CommandResult> Connect(AzConnectedvmwareVcenterConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzConnectedvmwareVcenterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVcenterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzConnectedvmwareVcenterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVcenterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzConnectedvmwareVcenterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedvmwareVcenterShowOptions(), token);
    }
}