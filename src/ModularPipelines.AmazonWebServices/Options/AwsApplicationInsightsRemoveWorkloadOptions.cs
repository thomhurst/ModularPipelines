using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("application-insights", "remove-workload")]
public record AwsApplicationInsightsRemoveWorkloadOptions(
[property: CliOption("--resource-group-name")] string ResourceGroupName,
[property: CliOption("--component-name")] string ComponentName,
[property: CliOption("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}