using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage")]
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