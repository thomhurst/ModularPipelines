using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbTable
{
    public AzCosmosdbTable(
        AzCosmosdbTableRestorableResource restorableResource,
        AzCosmosdbTableRestorableTable restorableTable,
        AzCosmosdbTableRetrieveLatestBackupTime retrieveLatestBackupTime,
        AzCosmosdbTableThroughput throughput,
        ICommand internalCommand
    )
    {
        RestorableResource = restorableResource;
        RestorableTable = restorableTable;
        RetrieveLatestBackupTimeCommands = retrieveLatestBackupTime;
        Throughput = throughput;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbTableRestorableResource RestorableResource { get; }

    public AzCosmosdbTableRestorableTable RestorableTable { get; }

    public AzCosmosdbTableRetrieveLatestBackupTime RetrieveLatestBackupTimeCommands { get; }

    public AzCosmosdbTableThroughput Throughput { get; }

    public async Task<CommandResult> Create(AzCosmosdbTableCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbTableDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbTableExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbTableListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzCosmosdbTableRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RetrieveLatestBackupTime(AzCosmosdbTableRetrieveLatestBackupTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbTableShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}