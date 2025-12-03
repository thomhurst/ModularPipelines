using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "delete-scaling-policy")]
public record AwsGameliftDeleteScalingPolicyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--fleet-id")] string FleetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}