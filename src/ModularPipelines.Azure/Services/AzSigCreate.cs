using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig")]
public class AzSigCreate
{
    public AzSigCreate(
        AzSigCreateImageGallery imageGallery
    )
    {
        ImageGallery = imageGallery;
    }

    public AzSigCreateImageGallery ImageGallery { get; }
}