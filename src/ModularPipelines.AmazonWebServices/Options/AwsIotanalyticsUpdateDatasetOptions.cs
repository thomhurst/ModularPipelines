using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "update-dataset")]
public record AwsIotanalyticsUpdateDatasetOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--triggers")]
    public string[]? Triggers { get; set; }

    [CommandSwitch("--content-delivery-rules")]
    public string[]? ContentDeliveryRules { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--versioning-configuration")]
    public string? VersioningConfiguration { get; set; }

    [CommandSwitch("--late-data-rules")]
    public string[]? LateDataRules { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}