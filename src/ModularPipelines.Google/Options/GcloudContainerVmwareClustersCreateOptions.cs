using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "vmware", "clusters", "create")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Cluster { get; set; }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Location { get; set; }

    [CommandSwitch("--version")]
    public new string Version { get; set; }

    [CommandSwitch("--admin-cluster-membership")]
    public string AdminClusterMembership { get; set; }

    [CommandSwitch("--admin-cluster-membership-location")]
    public string AdminClusterMembershipLocation { get; set; }

    [CommandSwitch("--admin-cluster-membership-project")]
    public string AdminClusterMembershipProject { get; set; }

    [CommandSwitch("--control-plane-vip")]
    public string ControlPlaneVip { get; set; }

    [CommandSwitch("--ingress-vip")]
    public string IngressVip { get; set; }

    [CommandSwitch("--metal-lb-config-address-pools")]
    public string[] MetalLbConfigAddressPools { get; set; }

    [CommandSwitch("--control-plane-node-port")]
    public string ControlPlaneNodePort { get; set; }

    [CommandSwitch("--ingress-http-node-port")]
    public string IngressHttpNodePort { get; set; }

    [CommandSwitch("--ingress-https-node-port")]
    public string IngressHttpsNodePort { get; set; }

    [CommandSwitch("--konnectivity-server-node-port")]
    public string KonnectivityServerNodePort { get; set; }

    [CommandSwitch("--f5-config-address")]
    public string F5ConfigAddress { get; set; }

    [CommandSwitch("--f5-config-partition")]
    public string F5ConfigPartition { get; set; }

    [CommandSwitch("--f5-config-snat-pool")]
    public string F5ConfigSnatPool { get; set; }

    [CommandSwitch("--pod-address-cidr-blocks")]
    public string PodAddressCidrBlocks { get; set; }

    [CommandSwitch("--service-address-cidr-blocks")]
    public string ServiceAddressCidrBlocks { get; set; }

    [CommandSwitch("--control-plane-ip-block")]
    public string[] ControlPlaneIpBlock { get; set; }

    [CommandSwitch("--dns-search-domains")]
    public string[] DnsSearchDomains { get; set; }

    [CommandSwitch("--dns-servers")]
    public string[] DnsServers { get; set; }

    [CommandSwitch("--ntp-servers")]
    public string[] NtpServers { get; set; }

    [BooleanCommandSwitch("--enable-dhcp")]
    public bool EnableDhcp { get; set; }

    [CommandSwitch("--static-ip-config-ip-blocks")]
    public string[] StaticIpConfigIpBlocks { get; set; }

    [CommandSwitch("--admin-users")]
    public string? AdminUsers { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--disable-aag-config")]
    public bool? DisableAagConfig { get; set; }

    [BooleanCommandSwitch("--disable-vsphere-csi")]
    public bool? DisableVsphereCsi { get; set; }

    [BooleanCommandSwitch("--enable-auto-repair")]
    public bool? EnableAutoRepair { get; set; }

    [BooleanCommandSwitch("--enable-vm-tracking")]
    public bool? EnableVmTracking { get; set; }

    [CommandSwitch("--upgrade-policy")]
    public string[]? UpgradePolicy { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--cpus")]
    public string? Cpus { get; set; }

    [BooleanCommandSwitch("--enable-auto-resize")]
    public bool? EnableAutoResize { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--replicas")]
    public string? Replicas { get; set; }

    [BooleanCommandSwitch("--disable-control-plane-v2")]
    public bool? DisableControlPlaneV2 { get; set; }

    [BooleanCommandSwitch("--enable-control-plane-v2")]
    public bool? EnableControlPlaneV2 { get; set; }

    [BooleanCommandSwitch("--enable-advanced-networking")]
    public bool? EnableAdvancedNetworking { get; set; }

    [BooleanCommandSwitch("--enable-dataplane-v2")]
    public bool? EnableDataplaneV2 { get; set; }

    [CommandSwitch("--vcenter-ca-cert-data")]
    public string? VcenterCaCertData { get; set; }

    [CommandSwitch("--vcenter-cluster")]
    public string? VcenterCluster { get; set; }

    [CommandSwitch("--vcenter-datacenter")]
    public string? VcenterDatacenter { get; set; }

    [CommandSwitch("--vcenter-datastore")]
    public string? VcenterDatastore { get; set; }

    [CommandSwitch("--vcenter-folder")]
    public string? VcenterFolder { get; set; }

    [CommandSwitch("--vcenter-resource-pool")]
    public string? VcenterResourcePool { get; set; }

    [CommandSwitch("--vcenter-storage-policy-name")]
    public string? VcenterStoragePolicyName { get; set; }
}
