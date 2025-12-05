using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "dedicated", "create")]
public record GcloudComputeInterconnectsAttachmentsDedicatedCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--interconnect")] string Interconnect,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--candidate-ipv6-subnets")]
    public string[]? CandidateIpv6Subnets { get; set; }

    [CliOption("--candidate-subnets")]
    public string[]? CandidateSubnets { get; set; }

    [CliOption("--cloud-router-ipv6-interface-id")]
    public string? CloudRouterIpv6InterfaceId { get; set; }

    [CliOption("--customer-router-ipv6-interface-id")]
    public string? CustomerRouterIpv6InterfaceId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--ipsec-internal-addresses")]
    public string[]? IpsecInternalAddresses { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }

    [CliOption("--subnet-length")]
    public string? SubnetLength { get; set; }

    [CliOption("--vlan")]
    public string? Vlan { get; set; }
}