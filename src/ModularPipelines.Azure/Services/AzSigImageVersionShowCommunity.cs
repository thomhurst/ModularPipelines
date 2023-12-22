using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version")]
public class AzSigImageVersionShowCommunity
{
    public AzSigImageVersionShowCommunity(
        AzSigImageVersionShowCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigImageVersionShowCommunityImageGallery ImageGallery { get; }
}