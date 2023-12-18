using System.Diagnostics.CodeAnalysis;

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