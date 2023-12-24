using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "create-dataset-content")]
public record AwsIotanalyticsCreateDatasetContentOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}