using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file")]
public class AzStorageFileMetadata
{
    public AzStorageFileMetadata(
        AzStorageFileMetadataShow show,
        AzStorageFileMetadataUpdate update,
        ICommand internalCommand
    )
    {
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageFileMetadataShow ShowCommands { get; }

    public AzStorageFileMetadataUpdate UpdateCommands { get; }

    public async Task<CommandResult> Show(AzStorageFileMetadataShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageFileMetadataUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}