using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ce", "update-anomaly-monitor")]
public record AwsCeUpdateAnomalyMonitorOptions(
[property: CommandSwitch("--monitor-arn")] string MonitorArn
) : AwsOptions
{
    [CommandSwitch("--monitor-name")]
    public string? MonitorName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}