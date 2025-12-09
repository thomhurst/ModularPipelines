using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file")]
public class AzStorageFileCopy
{
    public AzStorageFileCopy(
        AzStorageFileCopyCancel cancel,
        AzStorageFileCopyStart start,
        AzStorageFileCopyStartBatch startBatch,
        ICommand internalCommand
    )
    {
        CancelCommands = cancel;
        StartCommands = start;
        StartBatchCommands = startBatch;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageFileCopyCancel CancelCommands { get; }

    public AzStorageFileCopyStart StartCommands { get; }

    public AzStorageFileCopyStartBatch StartBatchCommands { get; }

    public async Task<CommandResult> Cancel(AzStorageFileCopyCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzStorageFileCopyStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBatch(AzStorageFileCopyStartBatchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageFileCopyStartBatchOptions(), token);
    }
}