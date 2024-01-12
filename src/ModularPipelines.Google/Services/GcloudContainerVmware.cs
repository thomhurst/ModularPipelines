using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container")]
public class GcloudContainerVmware
{
    public GcloudContainerVmware(
        GcloudContainerVmwareAdminClusters adminClusters,
        GcloudContainerVmwareClusters clusters,
        GcloudContainerVmwareNodePools nodePools,
        GcloudContainerVmwareOperations operations
    )
    {
        AdminClusters = adminClusters;
        Clusters = clusters;
        NodePools = nodePools;
        Operations = operations;
    }

    public GcloudContainerVmwareAdminClusters AdminClusters { get; }

    public GcloudContainerVmwareClusters Clusters { get; }

    public GcloudContainerVmwareNodePools NodePools { get; }

    public GcloudContainerVmwareOperations Operations { get; }
}