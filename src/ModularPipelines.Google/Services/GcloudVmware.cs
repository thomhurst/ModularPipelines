using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudVmware
{
    public GcloudVmware(
        GcloudVmwareDnsBindPermission dnsBindPermission,
        GcloudVmwareLocations locations,
        GcloudVmwareNetworkPeerings networkPeerings,
        GcloudVmwareNetworkPolicies networkPolicies,
        GcloudVmwareNetworks networks,
        GcloudVmwareNodeTypes nodeTypes,
        GcloudVmwareOperations operations,
        GcloudVmwarePrivateClouds privateClouds,
        GcloudVmwarePrivateConnections privateConnections
    )
    {
        DnsBindPermission = dnsBindPermission;
        Locations = locations;
        NetworkPeerings = networkPeerings;
        NetworkPolicies = networkPolicies;
        Networks = networks;
        NodeTypes = nodeTypes;
        Operations = operations;
        PrivateClouds = privateClouds;
        PrivateConnections = privateConnections;
    }

    public GcloudVmwareDnsBindPermission DnsBindPermission { get; }

    public GcloudVmwareLocations Locations { get; }

    public GcloudVmwareNetworkPeerings NetworkPeerings { get; }

    public GcloudVmwareNetworkPolicies NetworkPolicies { get; }

    public GcloudVmwareNetworks Networks { get; }

    public GcloudVmwareNodeTypes NodeTypes { get; }

    public GcloudVmwareOperations Operations { get; }

    public GcloudVmwarePrivateClouds PrivateClouds { get; }

    public GcloudVmwarePrivateConnections PrivateConnections { get; }
}