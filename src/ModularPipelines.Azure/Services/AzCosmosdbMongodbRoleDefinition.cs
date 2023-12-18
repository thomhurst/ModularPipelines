using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "mongodb", "role")]
public class AzCosmosdbMongodbRoleDefinition
{
    public AzCosmosdbMongodbRoleDefinition(
        AzCosmosdbMongodbRoleDefinitionCreate create,
        AzCosmosdbMongodbRoleDefinitionDelete delete,
        AzCosmosdbMongodbRoleDefinitionExists exists,
        AzCosmosdbMongodbRoleDefinitionList list,
        AzCosmosdbMongodbRoleDefinitionShow show,
        AzCosmosdbMongodbRoleDefinitionUpdate update,
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

    public AzCosmosdbMongodbRoleDefinitionCreate CreateCommands { get; }

    public AzCosmosdbMongodbRoleDefinitionDelete DeleteCommands { get; }

    public AzCosmosdbMongodbRoleDefinitionExists ExistsCommands { get; }

    public AzCosmosdbMongodbRoleDefinitionList ListCommands { get; }

    public AzCosmosdbMongodbRoleDefinitionShow ShowCommands { get; }

    public AzCosmosdbMongodbRoleDefinitionUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzCosmosdbMongodbRoleDefinitionCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbMongodbRoleDefinitionDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzCosmosdbMongodbRoleDefinitionExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzCosmosdbMongodbRoleDefinitionListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzCosmosdbMongodbRoleDefinitionShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbMongodbRoleDefinitionUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}

