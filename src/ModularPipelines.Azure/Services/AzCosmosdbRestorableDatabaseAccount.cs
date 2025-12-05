using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb")]
public class AzCosmosdbRestorableDatabaseAccount
{
    public AzCosmosdbRestorableDatabaseAccount(
        AzCosmosdbRestorableDatabaseAccountList list,
        AzCosmosdbRestorableDatabaseAccountShow show,
        ICommand internalCommand
    )
    {
        ListCommands = list;
        ShowCommands = show;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbRestorableDatabaseAccountList ListCommands { get; }

    public AzCosmosdbRestorableDatabaseAccountShow ShowCommands { get; }

    public async Task<CommandResult> List(AzCosmosdbRestorableDatabaseAccountListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbRestorableDatabaseAccountListOptions(), token);
    }

    public async Task<CommandResult> Show(AzCosmosdbRestorableDatabaseAccountShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbRestorableDatabaseAccountShowOptions(), token);
    }
}