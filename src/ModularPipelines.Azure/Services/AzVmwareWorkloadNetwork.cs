using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware")]
public class AzVmwareWorkloadNetwork
{
    public AzVmwareWorkloadNetwork(
        AzVmwareWorkloadNetworkDhcp dhcp,
        AzVmwareWorkloadNetworkDnsService dnsService,
        AzVmwareWorkloadNetworkDnsZone dnsZone,
        AzVmwareWorkloadNetworkGateway gateway,
        AzVmwareWorkloadNetworkPortMirroring portMirroring,
        AzVmwareWorkloadNetworkPublicIp publicIp,
        AzVmwareWorkloadNetworkSegment segment,
        AzVmwareWorkloadNetworkVm vm,
        AzVmwareWorkloadNetworkVmGroup vmGroup
    )
    {
        Dhcp = dhcp;
        DnsService = dnsService;
        DnsZone = dnsZone;
        Gateway = gateway;
        PortMirroring = portMirroring;
        PublicIp = publicIp;
        Segment = segment;
        Vm = vm;
        VmGroup = vmGroup;
    }

    public AzVmwareWorkloadNetworkDhcp Dhcp { get; }

    public AzVmwareWorkloadNetworkDnsService DnsService { get; }

    public AzVmwareWorkloadNetworkDnsZone DnsZone { get; }

    public AzVmwareWorkloadNetworkGateway Gateway { get; }

    public AzVmwareWorkloadNetworkPortMirroring PortMirroring { get; }

    public AzVmwareWorkloadNetworkPublicIp PublicIp { get; }

    public AzVmwareWorkloadNetworkSegment Segment { get; }

    public AzVmwareWorkloadNetworkVm Vm { get; }

    public AzVmwareWorkloadNetworkVmGroup VmGroup { get; }
}