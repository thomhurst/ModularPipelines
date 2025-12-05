using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "disassociate-connect-peer")]
public record AwsNetworkmanagerDisassociateConnectPeerOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--connect-peer-id")] string ConnectPeerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}