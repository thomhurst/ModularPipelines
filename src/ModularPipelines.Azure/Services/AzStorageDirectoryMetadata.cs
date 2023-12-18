using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory")]
public class AzStorageDirectoryMetadata
{
    public AzStorageDirectoryMetadata(
        AzStorageDirectoryMetadataShow show,
        AzStorageDirectoryMetadataUpdate update,
        ICommand internalCommand
    )
    {
        ShowCommands = show;
        UpdateCommands = update;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzStorageDirectoryMetadataShow ShowCommands { get; }

    public AzStorageDirectoryMetadataUpdate UpdateCommands { get; }

    public async Task<CommandResult> Show(AzStorageDirectoryMetadataShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzStorageDirectoryMetadataUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}