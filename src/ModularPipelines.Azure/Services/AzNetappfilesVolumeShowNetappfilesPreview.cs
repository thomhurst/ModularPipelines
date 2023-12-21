using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "show")]
public class AzNetappfilesVolumeShowNetappfilesPreview
{
    public AzNetappfilesVolumeShowNetappfilesPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzNetappfilesVolumeShowNetappfilesPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeShowNetappfilesPreviewExtensionOptions(), token);
    }
}