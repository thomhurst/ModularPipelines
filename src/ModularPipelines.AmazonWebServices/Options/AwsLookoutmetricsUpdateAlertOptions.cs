using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutmetrics", "update-alert")]
public record AwsLookoutmetricsUpdateAlertOptions(
[property: CommandSwitch("--alert-arn")] string AlertArn
) : AwsOptions
{
    [CommandSwitch("--alert-description")]
    public string? AlertDescription { get; set; }

    [CommandSwitch("--alert-sensitivity-threshold")]
    public int? AlertSensitivityThreshold { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--alert-filters")]
    public string? AlertFilters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}