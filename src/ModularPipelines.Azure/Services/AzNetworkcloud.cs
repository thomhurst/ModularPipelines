using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNetworkcloud
{
    public AzNetworkcloud(
        AzNetworkcloudBaremetalmachine baremetalmachine,
        AzNetworkcloudCloudservicesnetwork cloudservicesnetwork,
        AzNetworkcloudCluster cluster,
        AzNetworkcloudClustermanager clustermanager,
        AzNetworkcloudKubernetescluster kubernetescluster,
        AzNetworkcloudL2network l2network,
        AzNetworkcloudL3network l3network,
        AzNetworkcloudRack rack,
        AzNetworkcloudRacksku racksku,
        AzNetworkcloudStorageappliance storageappliance,
        AzNetworkcloudTrunkednetwork trunkednetwork,
        AzNetworkcloudVirtualmachine virtualmachine,
        AzNetworkcloudVolume volume
    )
    {
        Baremetalmachine = baremetalmachine;
        Cloudservicesnetwork = cloudservicesnetwork;
        Cluster = cluster;
        Clustermanager = clustermanager;
        Kubernetescluster = kubernetescluster;
        L2network = l2network;
        L3network = l3network;
        Rack = rack;
        Racksku = racksku;
        Storageappliance = storageappliance;
        Trunkednetwork = trunkednetwork;
        Virtualmachine = virtualmachine;
        Volume = volume;
    }

    public AzNetworkcloudBaremetalmachine Baremetalmachine { get; }

    public AzNetworkcloudCloudservicesnetwork Cloudservicesnetwork { get; }

    public AzNetworkcloudCluster Cluster { get; }

    public AzNetworkcloudClustermanager Clustermanager { get; }

    public AzNetworkcloudKubernetescluster Kubernetescluster { get; }

    public AzNetworkcloudL2network L2network { get; }

    public AzNetworkcloudL3network L3network { get; }

    public AzNetworkcloudRack Rack { get; }

    public AzNetworkcloudRacksku Racksku { get; }

    public AzNetworkcloudStorageappliance Storageappliance { get; }

    public AzNetworkcloudTrunkednetwork Trunkednetwork { get; }

    public AzNetworkcloudVirtualmachine Virtualmachine { get; }

    public AzNetworkcloudVolume Volume { get; }
}