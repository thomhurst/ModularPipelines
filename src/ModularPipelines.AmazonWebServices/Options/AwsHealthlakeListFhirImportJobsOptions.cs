using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthlake", "list-fhir-import-jobs")]
public record AwsHealthlakeListFhirImportJobsOptions(
[property: CliOption("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--job-status")]
    public string? JobStatus { get; set; }

    [CliOption("--submitted-before")]
    public long? SubmittedBefore { get; set; }

    [CliOption("--submitted-after")]
    public long? SubmittedAfter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}