using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition", "list-community")]
public class AzSigImageDefinitionListCommunityImageGallery
{
    public AzSigImageDefinitionListCommunityImageGallery(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Extension(AzSigImageDefinitionListCommunityImageGalleryExtensionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSigImageDefinitionListCommunityImageGalleryExtensionOptions(), token);
    }
}