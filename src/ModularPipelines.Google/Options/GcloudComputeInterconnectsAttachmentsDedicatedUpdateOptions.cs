using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "attachments", "dedicated", "update")]
public record GcloudComputeInterconnectsAttachmentsDedicatedUpdateOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--bandwidth")]
    public string? Bandwidth { get; set; }

    [CliOption("--candidate-ipv6-subnets")]
    public string[]? CandidateIpv6Subnets { get; set; }

    [CliOption("--cloud-router-ipv6-interface-id")]
    public string? CloudRouterIpv6InterfaceId { get; set; }

    [CliOption("--customer-router-ipv6-interface-id")]
    public string? CustomerRouterIpv6InterfaceId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-admin")]
    public bool? EnableAdmin { get; set; }

    [CliOption("--mtu")]
    public string? Mtu { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--stack-type")]
    public string? StackType { get; set; }
}