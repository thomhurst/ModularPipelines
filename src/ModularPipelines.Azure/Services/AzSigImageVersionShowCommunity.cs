using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-version")]
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