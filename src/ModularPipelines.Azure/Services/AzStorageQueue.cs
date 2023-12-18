using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageQueue
{
    public AzStorageQueue(
        AzStorageQueueMetadata metadata,
        AzStorageQueuePolicy policy,
        ICommand internalCommand
    )
    {
        Metadata = metadata;
        Policy = policy;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageQueueMetadata Metadata { get; }

    public AzStorageQueuePolicy Policy { get; }

    public async Task<CommandResult> Create(AzStorageQueueCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzStorageQueueDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exists(AzStorageQueueExistsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GenerateSas(AzStorageQueueGenerateSasOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzStorageQueueListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageQueueListOptions(), token);
    }

    public async Task<CommandResult> Stats(AzStorageQueueStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageQueueStatsOptions(), token);
    }
}