using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "create-dataset")]
public record AwsRekognitionCreateDatasetOptions(
[property: CliOption("--dataset-type")] string DatasetType,
[property: CliOption("--project-arn")] string ProjectArn
) : AwsOptions
{
    [CliOption("--dataset-source")]
    public string? DatasetSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}