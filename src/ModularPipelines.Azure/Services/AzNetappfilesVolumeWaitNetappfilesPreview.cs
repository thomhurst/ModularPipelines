using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "wait")]
public class AzNetappfilesVolumeWaitNetappfilesPreview
{
    public AzNetappfilesVolumeWaitNetappfilesPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzNetappfilesVolumeWaitNetappfilesPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeWaitNetappfilesPreviewExtensionOptions(), token);
    }
}