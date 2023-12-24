using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "delete-bgp-peer")]
public record AwsDirectconnectDeleteBgpPeerOptions : AwsOptions
{
    [CommandSwitch("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CommandSwitch("--asn")]
    public int? Asn { get; set; }

    [CommandSwitch("--customer-address")]
    public string? CustomerAddress { get; set; }

    [CommandSwitch("--bgp-peer-id")]
    public string? BgpPeerId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}