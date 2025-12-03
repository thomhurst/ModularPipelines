using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-vpn-connection-route")]
public record AwsEc2CreateVpnConnectionRouteOptions(
[property: CliOption("--destination-cidr-block")] string DestinationCidrBlock,
[property: CliOption("--vpn-connection-id")] string VpnConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}