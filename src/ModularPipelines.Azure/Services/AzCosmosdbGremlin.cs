using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbGremlin
{
    public AzCosmosdbGremlin(
        AzCosmosdbGremlinDatabase database,
        AzCosmosdbGremlinGraph graph,
        AzCosmosdbGremlinRestorableDatabase restorableDatabase,
        AzCosmosdbGremlinRestorableGraph restorableGraph,
        AzCosmosdbGremlinRestorableResource restorableResource,
        AzCosmosdbGremlinRetrieveLatestBackupTime retrieveLatestBackupTime,
        ICommand internalCommand
    )
    {
        Database = database;
        Graph = graph;
        RestorableDatabase = restorableDatabase;
        RestorableGraph = restorableGraph;
        RestorableResource = restorableResource;
        RetrieveLatestBackupTimeCommands = retrieveLatestBackupTime;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbGremlinDatabase Database { get; }

    public AzCosmosdbGremlinGraph Graph { get; }

    public AzCosmosdbGremlinRestorableDatabase RestorableDatabase { get; }

    public AzCosmosdbGremlinRestorableGraph RestorableGraph { get; }

    public AzCosmosdbGremlinRestorableResource RestorableResource { get; }

    public AzCosmosdbGremlinRetrieveLatestBackupTime RetrieveLatestBackupTimeCommands { get; }

    public async Task<CommandResult> RetrieveLatestBackupTime(AzCosmosdbGremlinRetrieveLatestBackupTimeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}