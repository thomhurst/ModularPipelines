using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "delete-bgp-peer")]
public record AwsDirectconnectDeleteBgpPeerOptions : AwsOptions
{
    [CliOption("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CliOption("--asn")]
    public int? Asn { get; set; }

    [CliOption("--customer-address")]
    public string? CustomerAddress { get; set; }

    [CliOption("--bgp-peer-id")]
    public string? BgpPeerId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}