using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb")]
public class AzCosmosdbService
{
    public AzCosmosdbService(
        AzCosmosdbServiceCreate create,
        AzCosmosdbServiceDelete delete,
        AzCosmosdbServiceList list,
        AzCosmosdbServiceShow show,
        AzCosmosdbServiceUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbServiceCreate CreateCommands { get; }

    public AzCosmosdbServiceDelete DeleteCommands { get; }

    public AzCosmosdbServiceList ListCommands { get; }

    public AzCosmosdbServiceShow ShowCommands { get; }

    public AzCosmosdbServiceUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzCosmosdbServiceCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbServiceDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbServiceListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbServiceShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbServiceUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}