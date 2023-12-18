using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage")]
public class AzStorageAzcopy
{
    public AzStorageAzcopy(
        AzStorageAzcopyBlob blob,
        ICommand internalCommand
    )
    {
        Blob = blob;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageAzcopyBlob Blob { get; }

    public async Task<CommandResult> RunCommand(AzStorageAzcopyRunCommandOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzStorageAzcopyRunCommandOptions(), token);
    }
}