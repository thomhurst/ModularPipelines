using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managed-cassandra")]
public class AzManagedCassandraDatacenter
{
    public AzManagedCassandraDatacenter(
        AzManagedCassandraDatacenterCreate create,
        AzManagedCassandraDatacenterDelete delete,
        AzManagedCassandraDatacenterList list,
        AzManagedCassandraDatacenterShow show,
        AzManagedCassandraDatacenterUpdate update,
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

    public AzManagedCassandraDatacenterCreate CreateCommands { get; }

    public AzManagedCassandraDatacenterDelete DeleteCommands { get; }

    public AzManagedCassandraDatacenterList ListCommands { get; }

    public AzManagedCassandraDatacenterShow ShowCommands { get; }

    public AzManagedCassandraDatacenterUpdate UpdateCommands { get; }

    public async Task<CommandResult> Create(AzManagedCassandraDatacenterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzManagedCassandraDatacenterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzManagedCassandraDatacenterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzManagedCassandraDatacenterShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzManagedCassandraDatacenterUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}