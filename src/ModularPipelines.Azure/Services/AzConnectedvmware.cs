using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConnectedvmware
{
    public AzConnectedvmware(
        AzConnectedvmwareCluster cluster,
        AzConnectedvmwareDatastore datastore,
        AzConnectedvmwareHost host,
        AzConnectedvmwareResourcePool resourcePool,
        AzConnectedvmwareVcenter vcenter,
        AzConnectedvmwareVirtualNetwork virtualNetwork,
        AzConnectedvmwareVm vm,
        AzConnectedvmwareVmTemplate vmTemplate
    )
    {
        Cluster = cluster;
        Datastore = datastore;
        Host = host;
        ResourcePool = resourcePool;
        Vcenter = vcenter;
        VirtualNetwork = virtualNetwork;
        Vm = vm;
        VmTemplate = vmTemplate;
    }

    public AzConnectedvmwareCluster Cluster { get; }

    public AzConnectedvmwareDatastore Datastore { get; }

    public AzConnectedvmwareHost Host { get; }

    public AzConnectedvmwareResourcePool ResourcePool { get; }

    public AzConnectedvmwareVcenter Vcenter { get; }

    public AzConnectedvmwareVirtualNetwork VirtualNetwork { get; }

    public AzConnectedvmwareVm Vm { get; }

    public AzConnectedvmwareVmTemplate VmTemplate { get; }
}