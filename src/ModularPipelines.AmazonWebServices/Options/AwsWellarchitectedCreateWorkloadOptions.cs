using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wellarchitected", "create-workload")]
public record AwsWellarchitectedCreateWorkloadOptions(
[property: CliOption("--workload-name")] string WorkloadName,
[property: CliOption("--description")] string Description,
[property: CliOption("--environment")] string Environment,
[property: CliOption("--lenses")] string[] Lenses
) : AwsOptions
{
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

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--discovery-config")]
    public string? DiscoveryConfig { get; set; }

    [CliOption("--applications")]
    public string[]? Applications { get; set; }

    [CliOption("--profile-arns")]
    public string[]? ProfileArns { get; set; }

    [CliOption("--review-template-arns")]
    public string[]? ReviewTemplateArns { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}