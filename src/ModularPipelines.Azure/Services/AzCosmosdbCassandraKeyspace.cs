using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "cassandra")]
public class AzCosmosdbCassandraKeyspace
{
    public AzCosmosdbCassandraKeyspace(
        AzCosmosdbCassandraKeyspaceThroughput throughput,
        ICommand internalCommand
    )
    {
        Throughput = throughput;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbCassandraKeyspaceThroughput Throughput { get; }

    public async Task<CommandResult> Create(AzCosmosdbCassandraKeyspaceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbCassandraKeyspaceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbCassandraKeyspaceExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbCassandraKeyspaceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbCassandraKeyspaceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

