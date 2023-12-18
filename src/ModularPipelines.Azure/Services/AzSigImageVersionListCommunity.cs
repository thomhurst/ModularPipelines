using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-version")]
public class AzSigImageVersionListCommunity
{
    public AzSigImageVersionListCommunity(
        AzSigImageVersionListCommunityImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigImageVersionListCommunityImageGallery ImageGallery { get; }
}

