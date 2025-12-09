using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-spot-fleet-requests")]
public record AwsEc2CancelSpotFleetRequestsOptions(
[property: CliOption("--spot-fleet-request-ids")] string[] SpotFleetRequestIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}