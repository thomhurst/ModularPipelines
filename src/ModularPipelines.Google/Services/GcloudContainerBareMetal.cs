using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("container")]
public class GcloudContainerBareMetal
{
    public GcloudContainerBareMetal(
        GcloudContainerBareMetalAdminClusters adminClusters,
        GcloudContainerBareMetalClusters clusters,
        GcloudContainerBareMetalNodePools nodePools,
        GcloudContainerBareMetalOperations operations
    )
    {
        AdminClusters = adminClusters;
        Clusters = clusters;
        NodePools = nodePools;
        Operations = operations;
    }

    public GcloudContainerBareMetalAdminClusters AdminClusters { get; }

    public GcloudContainerBareMetalClusters Clusters { get; }

    public GcloudContainerBareMetalNodePools NodePools { get; }

    public GcloudContainerBareMetalOperations Operations { get; }
}