using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagesync")]
public class AzStoragesyncSyncGroup
{
    public AzStoragesyncSyncGroup(
        AzStoragesyncSyncGroupCloudEndpoint cloudEndpoint,
        AzStoragesyncSyncGroupServerEndpoint serverEndpoint,
        ICommand internalCommand
    )
    {
        CloudEndpoint = cloudEndpoint;
        ServerEndpoint = serverEndpoint;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStoragesyncSyncGroupCloudEndpoint CloudEndpoint { get; }

    public AzStoragesyncSyncGroupServerEndpoint ServerEndpoint { get; }

    public async Task<CommandResult> Create(AzStoragesyncSyncGroupCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStoragesyncSyncGroupDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStoragesyncSyncGroupListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStoragesyncSyncGroupShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}