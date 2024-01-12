using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "interconnects", "attachments", "dedicated", "create")]
public record GcloudComputeInterconnectsAttachmentsDedicatedCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--interconnect")] string Interconnect,
[property: CommandSwitch("--router")] string Router
) : GcloudOptions
{
    [CommandSwitch("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CommandSwitch("--candidate-ipv6-subnets")]
    public string[]? CandidateIpv6Subnets { get; set; }

    [CommandSwitch("--candidate-subnets")]
    public string[]? CandidateSubnets { get; set; }

    [CommandSwitch("--cloud-router-ipv6-interface-id")]
    public string? CloudRouterIpv6InterfaceId { get; set; }

    [CommandSwitch("--customer-router-ipv6-interface-id")]
    public string? CustomerRouterIpv6InterfaceId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--ipsec-internal-addresses")]
    public string[]? IpsecInternalAddresses { get; set; }

    [CommandSwitch("--mtu")]
    public string? Mtu { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--stack-type")]
    public string? StackType { get; set; }

    [CommandSwitch("--subnet-length")]
    public string? SubnetLength { get; set; }

    [CommandSwitch("--vlan")]
    public string? Vlan { get; set; }
}