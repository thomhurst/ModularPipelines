using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("cosmosdb", "postgres", "configuration")]
public class AzCosmosdbPostgresConfigurationCoordinator
{
    public AzCosmosdbPostgresConfigurationCoordinator(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Show(AzCosmosdbPostgresConfigurationCoordinatorShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationCoordinatorShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzCosmosdbPostgresConfigurationCoordinatorUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationCoordinatorUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzCosmosdbPostgresConfigurationCoordinatorWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresConfigurationCoordinatorWaitOptions(), token);
    }
}