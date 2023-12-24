using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "create-alert")]
public record AwsLookoutmetricsCreateAlertOptions(
[property: CommandSwitch("--alert-name")] string AlertName,
[property: CommandSwitch("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--alert-sensitivity-threshold")]
    public int? AlertSensitivityThreshold { get; set; }

    [CommandSwitch("--alert-description")]
    public string? AlertDescription { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--alert-filters")]
    public string? AlertFilters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}