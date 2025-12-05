using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutmetrics", "update-alert")]
public record AwsLookoutmetricsUpdateAlertOptions(
[property: CliOption("--alert-arn")] string AlertArn
) : AwsOptions
{
    [CliOption("--alert-description")]
    public string? AlertDescription { get; set; }

    [CliOption("--alert-sensitivity-threshold")]
    public int? AlertSensitivityThreshold { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--alert-filters")]
    public string? AlertFilters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}