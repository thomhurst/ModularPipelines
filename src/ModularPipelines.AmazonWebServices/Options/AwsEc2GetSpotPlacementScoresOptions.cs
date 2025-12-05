using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "get-spot-placement-scores")]
public record AwsEc2GetSpotPlacementScoresOptions(
[property: CliOption("--target-capacity")] int TargetCapacity
) : AwsOptions
{
    [CliOption("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CliOption("--target-capacity-unit-type")]
    public string? TargetCapacityUnitType { get; set; }

    [CliOption("--region-names")]
    public string[]? RegionNames { get; set; }

    [CliOption("--instance-requirements-with-metadata")]
    public string? InstanceRequirementsWithMetadata { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}