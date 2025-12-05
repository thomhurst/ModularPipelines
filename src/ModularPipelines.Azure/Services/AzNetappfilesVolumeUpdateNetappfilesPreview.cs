using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("netappfiles", "volume", "update")]
public class AzNetappfilesVolumeUpdateNetappfilesPreview
{
    public AzNetappfilesVolumeUpdateNetappfilesPreview(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzNetappfilesVolumeUpdateNetappfilesPreviewExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetappfilesVolumeUpdateNetappfilesPreviewExtensionOptions(), token);
    }
}