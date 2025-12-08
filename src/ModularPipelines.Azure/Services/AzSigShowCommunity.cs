using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig")]
public class AzSigShowCommunity
{
    public AzSigShowCommunity(
        AzSigShowCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigShowCommunityImageGallery ImageGallery { get; }
}