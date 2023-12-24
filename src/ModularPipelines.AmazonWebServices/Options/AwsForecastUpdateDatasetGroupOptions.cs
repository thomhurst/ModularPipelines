using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("forecast", "update-dataset-group")]
public record AwsForecastUpdateDatasetGroupOptions(
[property: CommandSwitch("--dataset-group-arn")] string DatasetGroupArn,
[property: CommandSwitch("--dataset-arns")] string[] DatasetArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}