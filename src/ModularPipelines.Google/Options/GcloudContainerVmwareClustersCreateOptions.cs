using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "vmware", "clusters", "create")]
public record GcloudContainerVmwareClustersCreateOptions : GcloudOptions
{
    public GcloudContainerVmwareClustersCreateOptions(
        string cluster,
        string location,
        string version,
        string adminClusterMembership,
        string adminClusterMembershipLocation,
        string adminClusterMembershipProject,
        string controlPlaneVip,
        string ingressVip,
        string[] metalLbConfigAddressPools,
        string controlPlaneNodePort,
        string ingressHttpNodePort,
        string ingressHttpsNodePort,
        string konnectivityServerNodePort,
        string f5ConfigAddress,
        string f5ConfigPartition,
        string f5ConfigSnatPool,
        string podAddressCidrBlocks,
        string serviceAddressCidrBlocks,
        string[] controlPlaneIpBlock,
        string[] dnsSearchDomains,
        string[] dnsServers,
        string[] ntpServers,
        bool enableDhcp,
        string[] staticIpConfigIpBlocks
    )
    {
        Cluster = cluster;
        Location = location;
        Version = version;
        AdminClusterMembership = adminClusterMembership;
        AdminClusterMembershipLocation = adminClusterMembershipLocation;
        AdminClusterMembershipProject = adminClusterMembershipProject;
        ControlPlaneVip = controlPlaneVip;
        IngressVip = ingressVip;
        MetalLbConfigAddressPools = metalLbConfigAddressPools;
        ControlPlaneNodePort = controlPlaneNodePort;
        IngressHttpNodePort = ingressHttpNodePort;
        IngressHttpsNodePort = ingressHttpsNodePort;
        KonnectivityServerNodePort = konnectivityServerNodePort;
        F5ConfigAddress = f5ConfigAddress;
        F5ConfigPartition = f5ConfigPartition;
        F5ConfigSnatPool = f5ConfigSnatPool;
        PodAddressCidrBlocks = podAddressCidrBlocks;
        ServiceAddressCidrBlocks = serviceAddressCidrBlocks;
        ControlPlaneIpBlock = controlPlaneIpBlock;
        DnsSearchDomains = dnsSearchDomains;
        DnsServers = dnsServers;
        NtpServers = ntpServers;
        EnableDhcp = enableDhcp;
        StaticIpConfigIpBlocks = staticIpConfigIpBlocks;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Cluster { get; set; }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Location { get; set; }

    [CliOption("--version")]
    public new string Version { get; set; }

    [CliOption("--admin-cluster-membership")]
    public string AdminClusterMembership { get; set; }

    [CliOption("--admin-cluster-membership-location")]
    public string AdminClusterMembershipLocation { get; set; }

    [CliOption("--admin-cluster-membership-project")]
    public string AdminClusterMembershipProject { get; set; }

    [CliOption("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [CliOption("--ingress-vip")]
    public string IngressVip { get; set; }

    [CliOption("--metal-lb-config-address-pools")]
    public string[] MetalLbConfigAddressPools { get; set; }

    [CliOption("--control-plane-node-port")]
    public string ControlPlaneNodePort { get; set; }

    [CliOption("--ingress-http-node-port")]
    public string IngressHttpNodePort { get; set; }

    [CliOption("--ingress-https-node-port")]
    public string IngressHttpsNodePort { get; set; }

    [CliOption("--konnectivity-server-node-port")]
    public string KonnectivityServerNodePort { get; set; }

    [CliOption("--f5-config-address")]
    public string F5ConfigAddress { get; set; }

    [CliOption("--f5-config-partition")]
    public string F5ConfigPartition { get; set; }

    [CliOption("--f5-config-snat-pool")]
    public string F5ConfigSnatPool { get; set; }

    [CliOption("--pod-address-cidr-blocks")]
    public string PodAddressCidrBlocks { get; set; }

    [CliOption("--service-address-cidr-blocks")]
    public string ServiceAddressCidrBlocks { get; set; }

    [CliOption("--control-plane-ip-block")]
    public string[] ControlPlaneIpBlock { get; set; }

    [CliOption("--dns-search-domains")]
    public string[] DnsSearchDomains { get; set; }

    [CliOption("--dns-servers")]
    public string[] DnsServers { get; set; }

    [CliOption("--ntp-servers")]
    public string[] NtpServers { get; set; }

    [CliFlag("--enable-dhcp")]
    public bool EnableDhcp { get; set; }

    [CliOption("--static-ip-config-ip-blocks")]
    public string[] StaticIpConfigIpBlocks { get; set; }

    [CliOption("--admin-users")]
    public string? AdminUsers { get; set; }

    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--disable-aag-config")]
    public bool? DisableAagConfig { get; set; }

    [CliFlag("--disable-vsphere-csi")]
    public bool? DisableVsphereCsi { get; set; }

    [CliFlag("--enable-auto-repair")]
    public bool? EnableAutoRepair { get; set; }

    [CliFlag("--enable-vm-tracking")]
    public bool? EnableVmTracking { get; set; }

    [CliOption("--upgrade-policy")]
    public string[]? UpgradePolicy { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--cpus")]
    public string? Cpus { get; set; }

    [CliFlag("--enable-auto-resize")]
    public bool? EnableAutoResize { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--replicas")]
    public string? Replicas { get; set; }

    [CliFlag("--disable-control-plane-v2")]
    public bool? DisableControlPlaneV2 { get; set; }

    [CliFlag("--enable-control-plane-v2")]
    public bool? EnableControlPlaneV2 { get; set; }

    [CliFlag("--enable-advanced-networking")]
    public bool? EnableAdvancedNetworking { get; set; }

    [CliFlag("--enable-dataplane-v2")]
    public bool? EnableDataplaneV2 { get; set; }

    [CliOption("--vcenter-ca-cert-data")]
    public string? VcenterCaCertData { get; set; }

    [CliOption("--vcenter-cluster")]
    public string? VcenterCluster { get; set; }

    [CliOption("--vcenter-datacenter")]
    public string? VcenterDatacenter { get; set; }

    [CliOption("--vcenter-datastore")]
    public string? VcenterDatastore { get; set; }

    [CliOption("--vcenter-folder")]
    public string? VcenterFolder { get; set; }

    [CliOption("--vcenter-resource-pool")]
    public string? VcenterResourcePool { get; set; }

    [CliOption("--vcenter-storage-policy-name")]
    public string? VcenterStoragePolicyName { get; set; }
}
