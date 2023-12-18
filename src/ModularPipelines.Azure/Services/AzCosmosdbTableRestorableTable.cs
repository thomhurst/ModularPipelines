using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "table")]
public class AzCosmosdbTableRestorableTable
{
    public AzCosmosdbTableRestorableTable(
        AzCosmosdbTableRestorableTableList list,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbTableRestorableTableList ListCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbTableRestorableTableListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}