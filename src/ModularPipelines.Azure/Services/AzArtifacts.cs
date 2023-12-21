using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzArtifacts
{
    public AzArtifacts(
        AzArtifactsUniversal universal
    )
    {
        Universal = universal;
    }

    public AzArtifactsUniversal Universal { get; }
}