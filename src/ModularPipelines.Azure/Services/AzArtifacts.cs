using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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