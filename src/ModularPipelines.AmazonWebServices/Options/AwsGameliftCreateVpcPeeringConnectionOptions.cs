using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-vpc-peering-connection")]
public record AwsGameliftCreateVpcPeeringConnectionOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--peer-vpc-aws-account-id")] string PeerVpcAwsAccountId,
[property: CliOption("--peer-vpc-id")] string PeerVpcId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}