using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres", "configuration")]
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
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeShowOptions(), null, token);
    }

    public async Task<CommandResult> Update(AzCosmosdbPostgresConfigurationNodeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeUpdateOptions(), null, token);
    }

    public async Task<CommandResult> Wait(AzCosmosdbPostgresConfigurationNodeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationNodeWaitOptions(), null, token);
    }
}