using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra")]
public class AzManagedCassandraCluster
{
    public AzManagedCassandraCluster(
        AzManagedCassandraClusterBackup backup,
        AzManagedCassandraClusterCreate create,
        AzManagedCassandraClusterDeallocate deallocate,
        AzManagedCassandraClusterDelete delete,
        AzManagedCassandraClusterList list,
        AzManagedCassandraClusterShow show,
        AzManagedCassandraClusterUpdate update,
        ICommand internalCommand
    )
    {
        Backup = backup;
        CreateCommands = create;
        DeallocateCommands = deallocate;
        DeleteCommands = delete;
        ListCommands = list;
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzManagedCassandraClusterBackup Backup { get; }

    public AzManagedCassandraClusterCreate CreateCommands { get; }

    public AzManagedCassandraClusterDeallocate DeallocateCommands { get; }

    public AzManagedCassandraClusterDelete DeleteCommands { get; }

    public AzManagedCassandraClusterList ListCommands { get; }

    public AzManagedCassandraClusterShow ShowCommands { get; }

    public AzManagedCassandraClusterUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzManagedCassandraClusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deallocate(AzManagedCassandraClusterDeallocateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzManagedCassandraClusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> InvokeCommand(AzManagedCassandraClusterInvokeCommandOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzManagedCassandraClusterListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzManagedCassandraClusterListOptions(), token);
    }

    public async Task<CommandResult> Show(AzManagedCassandraClusterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzManagedCassandraClusterStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Status(AzManagedCassandraClusterStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzManagedCassandraClusterUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}