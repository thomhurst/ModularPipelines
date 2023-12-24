using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-workload")]
public record AwsWellarchitectedCreateWorkloadOptions(
[property: CommandSwitch("--workload-name")] string WorkloadName,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--lenses")] string[] Lenses
) : AwsOptions
{
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

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--discovery-config")]
    public string? DiscoveryConfig { get; set; }

    [CommandSwitch("--applications")]
    public string[]? Applications { get; set; }

    [CommandSwitch("--profile-arns")]
    public string[]? ProfileArns { get; set; }

    [CommandSwitch("--review-template-arns")]
    public string[]? ReviewTemplateArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}