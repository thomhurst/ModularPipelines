using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition")]
public class AzSigImageDefinitionListCommunity
{
    public AzSigImageDefinitionListCommunity(
        AzSigImageDefinitionListCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigImageDefinitionListCommunityImageGallery ImageGallery { get; }
}