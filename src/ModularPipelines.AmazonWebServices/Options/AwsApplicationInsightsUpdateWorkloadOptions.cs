using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "update-workload")]
public record AwsApplicationInsightsUpdateWorkloadOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName,
[property: CliOption("--workload-configuration")] string WorkloadConfiguration
) : AwsOptions
{
    [CliOption("--workload-id")]
    public string? WorkloadId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}