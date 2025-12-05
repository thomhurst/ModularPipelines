using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "delete-connect-peer")]
public record AwsNetworkmanagerDeleteConnectPeerOptions(
[property: CliOption("--connect-peer-id")] string ConnectPeerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}