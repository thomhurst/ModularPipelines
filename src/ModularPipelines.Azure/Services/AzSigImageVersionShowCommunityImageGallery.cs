using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version", "show-community")]
public class AzSigImageVersionShowCommunityImageGallery
{
    public AzSigImageVersionShowCommunityImageGallery(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzSigImageVersionShowCommunityImageGalleryExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageVersionShowCommunityImageGalleryExtensionOptions(), token);
    }
}