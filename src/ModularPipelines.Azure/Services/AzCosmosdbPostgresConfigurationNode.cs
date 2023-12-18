using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "configuration")]
public class AzCosmosdbPostgresConfigurationNode
{
    public AzCosmosdbPostgresConfigurationNode(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzCosmosdbPostgresConfigurationNodeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzCosmosdbPostgresConfigurationNodeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzCosmosdbPostgresConfigurationNodeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeWaitOptions(), token);
    }
}

