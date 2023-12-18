using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql")]
public class AzCosmosdbSqlRestorableContainer
{
    public AzCosmosdbSqlRestorableContainer(
        AzCosmosdbSqlRestorableContainerList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbSqlRestorableContainerList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbSqlRestorableContainerListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}