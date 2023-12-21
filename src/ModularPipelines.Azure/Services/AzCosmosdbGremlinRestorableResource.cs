using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "gremlin")]
public class AzCosmosdbGremlinRestorableResource
{
    public AzCosmosdbGremlinRestorableResource(
        AzCosmosdbGremlinRestorableResourceList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbGremlinRestorableResourceList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbGremlinRestorableResourceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}