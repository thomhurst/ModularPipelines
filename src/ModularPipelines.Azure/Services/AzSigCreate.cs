using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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