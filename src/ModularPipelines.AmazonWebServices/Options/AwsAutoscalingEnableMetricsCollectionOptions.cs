using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("autoscaling", "enable-metrics-collection")]
public record AwsAutoscalingEnableMetricsCollectionOptions(
[property: CommandSwitch("--auto-scaling-group-name")] string AutoScalingGroupName,
[property: CommandSwitch("--granularity")] string Granularity
) : AwsOptions
{
    [CommandSwitch("--metrics")]
    public string[]? Metrics { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}