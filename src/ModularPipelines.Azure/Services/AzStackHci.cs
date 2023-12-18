using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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