using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin")]
public class AzCosmosdbGremlinRestorableGraph
{
    public AzCosmosdbGremlinRestorableGraph(
        AzCosmosdbGremlinRestorableGraphList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbGremlinRestorableGraphList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbGremlinRestorableGraphListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}