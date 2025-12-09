using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rekognition", "list-dataset-entries")]
public record AwsRekognitionListDatasetEntriesOptions(
[property: CliOption("--dataset-arn")] string DatasetArn
) : AwsOptions
{
    [CliOption("--contains-labels")]
    public string[]? ContainsLabels { get; set; }

    [CliOption("--source-ref-contains")]
    public string? SourceRefContains { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}