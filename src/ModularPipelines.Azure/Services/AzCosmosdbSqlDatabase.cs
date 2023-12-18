using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql")]
public class AzCosmosdbSqlDatabase
{
    public AzCosmosdbSqlDatabase(
        AzCosmosdbSqlDatabaseThroughput throughput,
        ICommand internalCommand
    )
    {
        Throughput = throughput;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbSqlDatabaseThroughput Throughput { get; }

    public async Task<CommandResult> Create(AzCosmosdbSqlDatabaseCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbSqlDatabaseDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbSqlDatabaseExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbSqlDatabaseListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Merge(AzCosmosdbSqlDatabaseMergeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(AzCosmosdbSqlDatabaseRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbSqlDatabaseShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}