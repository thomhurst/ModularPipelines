using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "list-jobs")]
public record AwsBatchListJobsOptions : AwsOptions
{
    [CliOption("--job-queue")]
    public string? JobQueue { get; set; }

    [CliOption("--array-job-id")]
    public string? ArrayJobId { get; set; }

    [CliOption("--multi-node-job-id")]
    public string? MultiNodeJobId { get; set; }

    [CliOption("--job-status")]
    public string? JobStatus { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}