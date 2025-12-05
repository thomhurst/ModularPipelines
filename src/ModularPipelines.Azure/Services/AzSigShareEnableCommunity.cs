using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "share")]
public class AzSigShareEnableCommunity
{
    public AzSigShareEnableCommunity(
        AzSigShareEnableCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigShareEnableCommunityImageGallery ImageGallery { get; }
}