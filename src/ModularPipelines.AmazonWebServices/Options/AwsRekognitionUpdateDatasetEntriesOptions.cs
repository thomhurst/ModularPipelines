using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "update-dataset-entries")]
public record AwsRekognitionUpdateDatasetEntriesOptions(
[property: CliOption("--dataset-arn")] string DatasetArn,
[property: CliOption("--changes")] string Changes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}