using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "create-alert")]
public record AwsLookoutmetricsCreateAlertOptions(
[property: CliOption("--alert-name")] string AlertName,
[property: CliOption("--anomaly-detector-arn")] string AnomalyDetectorArn,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--alert-sensitivity-threshold")]
    public int? AlertSensitivityThreshold { get; set; }

    [CliOption("--alert-description")]
    public string? AlertDescription { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--alert-filters")]
    public string? AlertFilters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}