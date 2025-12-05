using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "delete-vpc-peering-connection")]
public record AwsGameliftDeleteVpcPeeringConnectionOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--vpc-peering-connection-id")] string VpcPeeringConnectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}