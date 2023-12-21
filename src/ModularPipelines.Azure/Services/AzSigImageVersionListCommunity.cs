using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

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