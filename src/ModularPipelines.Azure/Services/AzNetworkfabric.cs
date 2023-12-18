using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzNetworkfabric
{
    public AzNetworkfabric(
        AzNetworkfabricAcl acl,
        AzNetworkfabricController controller,
        AzNetworkfabricDevice device,
        AzNetworkfabricExternalnetwork externalnetwork,
        AzNetworkfabricFabric fabric,
        AzNetworkfabricInterface @Interface,
        AzNetworkfabricInternalnetwork internalnetwork,
        AzNetworkfabricInternetgateway internetgateway,
        AzNetworkfabricInternetgatewayrule internetgatewayrule,
        AzNetworkfabricIpcommunity ipcommunity,
        AzNetworkfabricIpextendedcommunity ipextendedcommunity,
        AzNetworkfabricIpprefix ipprefix,
        AzNetworkfabricL2domain l2domain,
        AzNetworkfabricL3domain l3domain,
        AzNetworkfabricNeighborgroup neighborgroup,
        AzNetworkfabricNni nni,
        AzNetworkfabricNpb npb,
        AzNetworkfabricRack rack,
        AzNetworkfabricRoutepolicy routepolicy,
        AzNetworkfabricTap tap,
        AzNetworkfabricTaprule taprule
    )
    {
        Acl = acl;
        Controller = controller;
        Device = device;
        Externalnetwork = externalnetwork;
        Fabric = fabric;
        Interface = @Interface;
        Internalnetwork = internalnetwork;
        Internetgateway = internetgateway;
        Internetgatewayrule = internetgatewayrule;
        Ipcommunity = ipcommunity;
        Ipextendedcommunity = ipextendedcommunity;
        Ipprefix = ipprefix;
        L2domain = l2domain;
        L3domain = l3domain;
        Neighborgroup = neighborgroup;
        Nni = nni;
        Npb = npb;
        Rack = rack;
        Routepolicy = routepolicy;
        Tap = tap;
        Taprule = taprule;
    }

    public AzNetworkfabricAcl Acl { get; }

    public AzNetworkfabricController Controller { get; }

    public AzNetworkfabricDevice Device { get; }

    public AzNetworkfabricExternalnetwork Externalnetwork { get; }

    public AzNetworkfabricFabric Fabric { get; }

    public AzNetworkfabricInterface Interface { get; }

    public AzNetworkfabricInternalnetwork Internalnetwork { get; }

    public AzNetworkfabricInternetgateway Internetgateway { get; }

    public AzNetworkfabricInternetgatewayrule Internetgatewayrule { get; }

    public AzNetworkfabricIpcommunity Ipcommunity { get; }

    public AzNetworkfabricIpextendedcommunity Ipextendedcommunity { get; }

    public AzNetworkfabricIpprefix Ipprefix { get; }

    public AzNetworkfabricL2domain L2domain { get; }

    public AzNetworkfabricL3domain L3domain { get; }

    public AzNetworkfabricNeighborgroup Neighborgroup { get; }

    public AzNetworkfabricNni Nni { get; }

    public AzNetworkfabricNpb Npb { get; }

    public AzNetworkfabricRack Rack { get; }

    public AzNetworkfabricRoutepolicy Routepolicy { get; }

    public AzNetworkfabricTap Tap { get; }

    public AzNetworkfabricTaprule Taprule { get; }
}