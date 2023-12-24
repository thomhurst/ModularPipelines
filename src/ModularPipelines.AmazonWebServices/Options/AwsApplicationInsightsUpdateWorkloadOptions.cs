using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("application-insights", "update-workload")]
public record AwsApplicationInsightsUpdateWorkloadOptions(
[property: CommandSwitch("--resource-group-name")] string ResourceGroupName,
[property: CommandSwitch("--component-name")] string ComponentName,
[property: CommandSwitch("--workload-configuration")] string WorkloadConfiguration
) : AwsOptions
{
    [CommandSwitch("--workload-id")]
    public string? WorkloadId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}