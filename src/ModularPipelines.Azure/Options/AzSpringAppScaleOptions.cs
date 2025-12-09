using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "app", "scale")]
public record AzSpringAppScaleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliOption("--instance-count")]
    public int? InstanceCount { get; set; }

    [CliOption("--max-replicas")]
    public string? MaxReplicas { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--min-replicas")]
    public string? MinReplicas { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scale-rule-auth")]
    public string? ScaleRuleAuth { get; set; }

    [CliOption("--scale-rule-http-concurrency")]
    public string? ScaleRuleHttpConcurrency { get; set; }

    [CliOption("--scale-rule-metadata")]
    public string? ScaleRuleMetadata { get; set; }

    [CliOption("--scale-rule-name")]
    public string? ScaleRuleName { get; set; }

    [CliOption("--scale-rule-type")]
    public string? ScaleRuleType { get; set; }
}