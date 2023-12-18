using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzStackHci
{
    public AzStackHci(
        AzStackHciArcSetting arcSetting,
        AzStackHciCluster cluster,
        AzStackHciExtension extension
    )
    {
        ArcSetting = arcSetting;
        Cluster = cluster;
        Extension = extension;
    }

    public AzStackHciArcSetting ArcSetting { get; }

    public AzStackHciCluster Cluster { get; }

    public AzStackHciExtension Extension { get; }
}