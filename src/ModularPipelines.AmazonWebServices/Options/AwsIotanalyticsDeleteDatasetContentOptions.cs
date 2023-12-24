using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "delete-dataset-content")]
public record AwsIotanalyticsDeleteDatasetContentOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}