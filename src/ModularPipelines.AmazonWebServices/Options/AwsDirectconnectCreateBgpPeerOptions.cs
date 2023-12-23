using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-bgp-peer")]
public record AwsDirectconnectCreateBgpPeerOptions : AwsOptions
{
    [CommandSwitch("--virtual-interface-id")]
    public string? VirtualInterfaceId { get; set; }

    [CommandSwitch("--new-bgp-peer")]
    public string? NewBgpPeer { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}