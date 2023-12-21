using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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