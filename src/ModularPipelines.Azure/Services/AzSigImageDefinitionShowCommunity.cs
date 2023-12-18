using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition")]
public class AzSigImageDefinitionShowCommunity
{
    public AzSigImageDefinitionShowCommunity(
        AzSigImageDefinitionShowCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigImageDefinitionShowCommunityImageGallery ImageGallery { get; }
}

