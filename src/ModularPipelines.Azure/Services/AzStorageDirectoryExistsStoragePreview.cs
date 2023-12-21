using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "directory", "exists")]
public class AzStorageDirectoryExistsStoragePreview
{
    public AzStorageDirectoryExistsStoragePreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzStorageDirectoryExistsStoragePreviewExtensionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}