using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageShare
{
    public AzStorageShare(
        AzStorageShareCloseHandle closeHandle,
        AzStorageShareListHandle listHandle,
        AzStorageShareMetadata metadata,
        AzStorageSharePolicy policy,
        ICommand internalCommand
    )
    {
        CloseHandleCommands = closeHandle;
        ListHandleCommands = listHandle;
        Metadata = metadata;
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageShareCloseHandle CloseHandleCommands { get; }

    public AzStorageShareListHandle ListHandleCommands { get; }

    public AzStorageShareMetadata Metadata { get; }

    public AzStorageSharePolicy Policy { get; }

    public async Task<CommandResult> CloseHandle(AzStorageShareCloseHandleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzStorageShareCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageShareDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageShareExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageShareGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageShareListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageShareListOptions(), token);
    }

    public async Task<CommandResult> ListHandle(AzStorageShareListHandleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzStorageShareShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Snapshot(AzStorageShareSnapshotOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stats(AzStorageShareStatsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageShareUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Url(AzStorageShareUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}