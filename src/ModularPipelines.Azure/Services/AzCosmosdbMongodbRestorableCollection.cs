using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb")]
public class AzCosmosdbMongodbRestorableCollection
{
    public AzCosmosdbMongodbRestorableCollection(
        AzCosmosdbMongodbRestorableCollectionList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbMongodbRestorableCollectionList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbMongodbRestorableCollectionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}