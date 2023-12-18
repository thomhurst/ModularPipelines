using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres")]
public class AzCosmosdbPostgresCluster
{
    public AzCosmosdbPostgresCluster(
        AzCosmosdbPostgresClusterServer server,
        ICommand internalCommand
    )
    {
        Server = server;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzCosmosdbPostgresClusterServer Server { get; }

    public async Task<CommandResult> Create(AzCosmosdbPostgresClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzCosmosdbPostgresClusterDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzCosmosdbPostgresClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterListOptions(), token);
    }

    public async Task<CommandResult> Promote(AzCosmosdbPostgresClusterPromoteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterPromoteOptions(), token);
    }

    public async Task<CommandResult> Restart(AzCosmosdbPostgresClusterRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterRestartOptions(), token);
    }

    public async Task<CommandResult> Show(AzCosmosdbPostgresClusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzCosmosdbPostgresClusterStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzCosmosdbPostgresClusterStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzCosmosdbPostgresClusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzCosmosdbPostgresClusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzCosmosdbPostgresClusterWaitOptions(), token);
    }
}

