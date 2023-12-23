using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "update-workload")]
public record AwsWellarchitectedUpdateWorkloadOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CommandSwitch("--workload-name")]
    public string? WorkloadName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CommandSwitch("--aws-regions")]
    public string[]? AwsRegions { get; set; }

    [CommandSwitch("--non-aws-regions")]
    public string[]? NonAwsRegions { get; set; }

    [CommandSwitch("--pillar-priorities")]
    public string[]? PillarPriorities { get; set; }

    [CommandSwitch("--architectural-design")]
    public string? ArchitecturalDesign { get; set; }

    [CommandSwitch("--review-owner")]
    public string? ReviewOwner { get; set; }

    [CommandSwitch("--industry-type")]
    public string? IndustryType { get; set; }

    [CommandSwitch("--industry")]
    public string? Industry { get; set; }

    [CommandSwitch("--notes")]
    public string? Notes { get; set; }

    [CommandSwitch("--improvement-status")]
    public string? ImprovementStatus { get; set; }

    [CommandSwitch("--discovery-config")]
    public string? DiscoveryConfig { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}