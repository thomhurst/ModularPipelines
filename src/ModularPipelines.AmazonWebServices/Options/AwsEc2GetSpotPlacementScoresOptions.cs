using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "get-spot-placement-scores")]
public record AwsEc2GetSpotPlacementScoresOptions(
[property: CommandSwitch("--target-capacity")] int TargetCapacity
) : AwsOptions
{
    [CommandSwitch("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CommandSwitch("--target-capacity-unit-type")]
    public string? TargetCapacityUnitType { get; set; }

    [CommandSwitch("--region-names")]
    public string[]? RegionNames { get; set; }

    [CommandSwitch("--instance-requirements-with-metadata")]
    public string? InstanceRequirementsWithMetadata { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}