using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rum", "batch-create-rum-metric-definitions")]
public record AwsRumBatchCreateRumMetricDefinitionsOptions(
[property: CommandSwitch("--app-monitor-name")] string AppMonitorName,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--metric-definitions")] string[] MetricDefinitions
) : AwsOptions
{
    [CommandSwitch("--destination-arn")]
    public string? DestinationArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}