using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-definition", "show-community")]
public class AzSigImageDefinitionShowCommunityImageGallery
{
    public AzSigImageDefinitionShowCommunityImageGallery(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzSigImageDefinitionShowCommunityImageGalleryExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionShowCommunityImageGalleryExtensionOptions(), token);
    }
}