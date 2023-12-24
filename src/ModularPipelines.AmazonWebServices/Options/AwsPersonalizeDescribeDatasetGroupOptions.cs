using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "describe-dataset-group")]
public record AwsPersonalizeDescribeDatasetGroupOptions(
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}