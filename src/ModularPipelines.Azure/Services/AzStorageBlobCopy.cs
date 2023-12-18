using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob")]
public class AzStorageBlobCopy
{
    public AzStorageBlobCopy(
        AzStorageBlobCopyStart start,
        ICommand internalCommand
    )
    {
        StartCommands = start;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageBlobCopyStart StartCommands { get; }

    public async Task<CommandResult> Cancel(AzStorageBlobCopyCancelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzStorageBlobCopyStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StartBatch(AzStorageBlobCopyStartBatchOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageBlobCopyStartBatchOptions(), token);
    }
}

