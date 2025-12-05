using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-bgp-peer")]
public record AwsDirectconnectCreateBgpPeerOptions : AwsOptions
{
    [CliOption("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CliOption("--new-bgp-peer")]
    public string? NewBgpPeer { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}