using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute-optimizer", "put-recommendation-preferences")]
public record AwsComputeOptimizerPutRecommendationPreferencesOptions(
[property: CommandSwitch("--resource-type")] string ResourceType
) : AwsOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--enhanced-infrastructure-metrics")]
    public string? EnhancedInfrastructureMetrics { get; set; }

    [CommandSwitch("--inferred-workload-types")]
    public string? InferredWorkloadTypes { get; set; }

    [CommandSwitch("--external-metrics-preference")]
    public string? ExternalMetricsPreference { get; set; }

    [CommandSwitch("--look-back-period")]
    public string? LookBackPeriod { get; set; }

    [CommandSwitch("--utilization-preferences")]
    public string[]? UtilizationPreferences { get; set; }

    [CommandSwitch("--preferred-resources")]
    public string[]? PreferredResources { get; set; }

    [CommandSwitch("--savings-estimation-mode")]
    public string? SavingsEstimationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}