using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "deregister-compute")]
public record AwsGameliftDeregisterComputeOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--compute-name")] string ComputeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}