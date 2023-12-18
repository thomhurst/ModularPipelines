using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzVmware
{
    public AzVmware(
        AzVmwareAddon addon,
        AzVmwareAuthorization authorization,
        AzVmwareCloudLink cloudLink,
        AzVmwareCluster cluster,
        AzVmwareDatastore datastore,
        AzVmwareGlobalReachConnection globalReachConnection,
        AzVmwareHcxEnterpriseSite hcxEnterpriseSite,
        AzVmwareLocation location,
        AzVmwarePlacementPolicy placementPolicy,
        AzVmwarePrivateCloud privateCloud,
        AzVmwareScriptCmdlet scriptCmdlet,
        AzVmwareScriptExecution scriptExecution,
        AzVmwareScriptPackage scriptPackage,
        AzVmwareVm vm,
        AzVmwareWorkloadNetwork workloadNetwork
    )
    {
        Addon = addon;
        Authorization = authorization;
        CloudLink = cloudLink;
        Cluster = cluster;
        Datastore = datastore;
        GlobalReachConnection = globalReachConnection;
        HcxEnterpriseSite = hcxEnterpriseSite;
        Location = location;
        PlacementPolicy = placementPolicy;
        PrivateCloud = privateCloud;
        ScriptCmdlet = scriptCmdlet;
        ScriptExecution = scriptExecution;
        ScriptPackage = scriptPackage;
        Vm = vm;
        WorkloadNetwork = workloadNetwork;
    }

    public AzVmwareAddon Addon { get; }

    public AzVmwareAuthorization Authorization { get; }

    public AzVmwareCloudLink CloudLink { get; }

    public AzVmwareCluster Cluster { get; }

    public AzVmwareDatastore Datastore { get; }

    public AzVmwareGlobalReachConnection GlobalReachConnection { get; }

    public AzVmwareHcxEnterpriseSite HcxEnterpriseSite { get; }

    public AzVmwareLocation Location { get; }

    public AzVmwarePlacementPolicy PlacementPolicy { get; }

    public AzVmwarePrivateCloud PrivateCloud { get; }

    public AzVmwareScriptCmdlet ScriptCmdlet { get; }

    public AzVmwareScriptExecution ScriptExecution { get; }

    public AzVmwareScriptPackage ScriptPackage { get; }

    public AzVmwareVm Vm { get; }

    public AzVmwareWorkloadNetwork WorkloadNetwork { get; }
}