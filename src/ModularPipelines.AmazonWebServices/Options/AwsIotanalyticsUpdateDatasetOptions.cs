using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "update-dataset")]
public record AwsIotanalyticsUpdateDatasetOptions(
[property: CliOption("--dataset-name")] string DatasetName,
[property: CliOption("--actions")] string[] Actions
) : AwsOptions
{
    [CliOption("--triggers")]
    public string[]? Triggers { get; set; }

    [CliOption("--content-delivery-rules")]
    public string[]? ContentDeliveryRules { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--versioning-configuration")]
    public string? VersioningConfiguration { get; set; }

    [CliOption("--late-data-rules")]
    public string[]? LateDataRules { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}