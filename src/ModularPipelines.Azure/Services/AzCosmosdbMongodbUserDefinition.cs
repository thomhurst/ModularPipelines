using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "mongodb", "user")]
public class AzCosmosdbMongodbUserDefinition
{
    public AzCosmosdbMongodbUserDefinition(
        AzCosmosdbMongodbUserDefinitionCreate create,
        AzCosmosdbMongodbUserDefinitionDelete delete,
        AzCosmosdbMongodbUserDefinitionExists exists,
        AzCosmosdbMongodbUserDefinitionList list,
        AzCosmosdbMongodbUserDefinitionShow show,
        AzCosmosdbMongodbUserDefinitionUpdate update,
        ICommand internalCommand
    )
    {
        CreateCommands = create;
        DeleteCommands = delete;
        ExistsCommands = exists;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbMongodbUserDefinitionCreate CreateCommands { get; }

    public AzCosmosdbMongodbUserDefinitionDelete DeleteCommands { get; }

    public AzCosmosdbMongodbUserDefinitionExists ExistsCommands { get; }

    public AzCosmosdbMongodbUserDefinitionList ListCommands { get; }

    public AzCosmosdbMongodbUserDefinitionShow ShowCommands { get; }

    public AzCosmosdbMongodbUserDefinitionUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzCosmosdbMongodbUserDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbMongodbUserDefinitionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbMongodbUserDefinitionExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbMongodbUserDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbMongodbUserDefinitionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbMongodbUserDefinitionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}