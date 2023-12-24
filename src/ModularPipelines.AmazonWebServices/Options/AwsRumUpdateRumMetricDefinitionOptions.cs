using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "update-rum-metric-definition")]
public record AwsRumUpdateRumMetricDefinitionOptions(
[property: CommandSwitch("--app-monitor-name")] string AppMonitorName,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--metric-definition")] string MetricDefinition,
[property: CommandSwitch("--metric-definition-id")] string MetricDefinitionId
) : AwsOptions
{
    [CommandSwitch("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}