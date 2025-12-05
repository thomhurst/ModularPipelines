using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "update-workload")]
public record AwsWellarchitectedUpdateWorkloadOptions(
[property: CliOption("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CliOption("--workload-name")]
    public string? WorkloadName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--aws-regions")]
    public string[]? AwsRegions { get; set; }

    [CliOption("--non-aws-regions")]
    public string[]? NonAwsRegions { get; set; }

    [CliOption("--pillar-priorities")]
    public string[]? PillarPriorities { get; set; }

    [CliOption("--architectural-design")]
    public string? ArchitecturalDesign { get; set; }

    [CliOption("--review-owner")]
    public string? ReviewOwner { get; set; }

    [CliOption("--industry-type")]
    public string? IndustryType { get; set; }

    [CliOption("--industry")]
    public string? Industry { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--improvement-status")]
    public string? ImprovementStatus { get; set; }

    [CliOption("--discovery-config")]
    public string? DiscoveryConfig { get; set; }

    [CliOption("--applications")]
    public string[]? Applications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}