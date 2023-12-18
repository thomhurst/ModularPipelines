using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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