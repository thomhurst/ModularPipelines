using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStoragesync
{
    public AzStoragesync(
        AzStoragesyncRegisteredServer registeredServer,
        AzStoragesyncSyncGroup syncGroup,
        ICommand internalCommand
    )
    {
        RegisteredServer = registeredServer;
        SyncGroup = syncGroup;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStoragesyncRegisteredServer RegisteredServer { get; }

    public AzStoragesyncSyncGroup SyncGroup { get; }

    public async Task<CommandResult> Create(AzStoragesyncCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStoragesyncDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStoragesyncDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzStoragesyncListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStoragesyncListOptions(), token);
    }

    public async Task<CommandResult> Show(AzStoragesyncShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStoragesyncShowOptions(), token);
    }
}