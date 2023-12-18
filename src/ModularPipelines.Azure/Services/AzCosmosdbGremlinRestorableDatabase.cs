using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin")]
public class AzCosmosdbGremlinRestorableDatabase
{
    public AzCosmosdbGremlinRestorableDatabase(
        AzCosmosdbGremlinRestorableDatabaseList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbGremlinRestorableDatabaseList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbGremlinRestorableDatabaseListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}