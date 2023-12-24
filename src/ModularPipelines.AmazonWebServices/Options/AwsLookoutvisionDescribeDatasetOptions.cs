using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "describe-dataset")]
public record AwsLookoutvisionDescribeDatasetOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}