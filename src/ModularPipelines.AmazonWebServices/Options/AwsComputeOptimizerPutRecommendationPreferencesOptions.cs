using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "put-recommendation-preferences")]
public record AwsComputeOptimizerPutRecommendationPreferencesOptions(
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--enhanced-infrastructure-metrics")]
    public string? EnhancedInfrastructureMetrics { get; set; }

    [CliOption("--inferred-workload-types")]
    public string? InferredWorkloadTypes { get; set; }

    [CliOption("--external-metrics-preference")]
    public string? ExternalMetricsPreference { get; set; }

    [CliOption("--look-back-period")]
    public string? LookBackPeriod { get; set; }

    [CliOption("--utilization-preferences")]
    public string[]? UtilizationPreferences { get; set; }

    [CliOption("--preferred-resources")]
    public string[]? PreferredResources { get; set; }

    [CliOption("--savings-estimation-mode")]
    public string? SavingsEstimationMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}