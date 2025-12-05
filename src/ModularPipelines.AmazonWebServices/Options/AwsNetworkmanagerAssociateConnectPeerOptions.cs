using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "associate-connect-peer")]
public record AwsNetworkmanagerAssociateConnectPeerOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--connect-peer-id")] string ConnectPeerId,
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--link-id")]
    public string? LinkId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}