using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

