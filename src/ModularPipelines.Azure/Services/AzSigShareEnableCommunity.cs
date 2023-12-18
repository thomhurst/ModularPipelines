using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "share")]
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

