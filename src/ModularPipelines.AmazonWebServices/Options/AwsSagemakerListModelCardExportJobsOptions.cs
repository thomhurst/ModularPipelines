using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "list-model-card-export-jobs")]
public record AwsSagemakerListModelCardExportJobsOptions(
[property: CliOption("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CliOption("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CliOption("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CliOption("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CliOption("--model-card-export-job-name-contains")]
    public string? ModelCardExportJobNameContains { get; set; }

    [CliOption("--status-equals")]
    public string? StatusEquals { get; set; }

    [CliOption("--sort-by")]
    public string? SortBy { get; set; }

    [CliOption("--sort-order")]
    public string? SortOrder { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}