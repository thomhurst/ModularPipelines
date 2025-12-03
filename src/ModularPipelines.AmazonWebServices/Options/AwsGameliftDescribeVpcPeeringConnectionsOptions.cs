using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "describe-vpc-peering-connections")]
public record AwsGameliftDescribeVpcPeeringConnectionsOptions : AwsOptions
{
    [CliOption("--fleet-id")]
    public string? FleetId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}