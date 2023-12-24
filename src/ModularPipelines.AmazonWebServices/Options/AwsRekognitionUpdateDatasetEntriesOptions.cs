using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "update-dataset-entries")]
public record AwsRekognitionUpdateDatasetEntriesOptions(
[property: CommandSwitch("--dataset-arn")] string DatasetArn,
[property: CommandSwitch("--changes")] string Changes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}