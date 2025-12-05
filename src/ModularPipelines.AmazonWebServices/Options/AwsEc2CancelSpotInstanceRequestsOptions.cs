using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-spot-instance-requests")]
public record AwsEc2CancelSpotInstanceRequestsOptions(
[property: CliOption("--spot-instance-request-ids")] string[] SpotInstanceRequestIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}