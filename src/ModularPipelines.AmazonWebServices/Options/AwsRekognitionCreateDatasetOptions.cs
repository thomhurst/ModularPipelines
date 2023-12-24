using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "create-dataset")]
public record AwsRekognitionCreateDatasetOptions(
[property: CommandSwitch("--dataset-type")] string DatasetType,
[property: CommandSwitch("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CommandSwitch("--dataset-source")]
    public string? DatasetSource { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}