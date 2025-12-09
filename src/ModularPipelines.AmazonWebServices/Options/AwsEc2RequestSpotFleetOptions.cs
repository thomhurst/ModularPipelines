using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "request-spot-fleet")]
public record AwsEc2RequestSpotFleetOptions(
[property: CliOption("--spot-fleet-request-config")] string SpotFleetRequestConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}