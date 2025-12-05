using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutvision", "list-dataset-entries")]
public record AwsLookoutvisionListDatasetEntriesOptions(
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CliOption("--anomaly-class")]
    public string? AnomalyClass { get; set; }

    [CliOption("--before-creation-date")]
    public long? BeforeCreationDate { get; set; }

    [CliOption("--after-creation-date")]
    public long? AfterCreationDate { get; set; }

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