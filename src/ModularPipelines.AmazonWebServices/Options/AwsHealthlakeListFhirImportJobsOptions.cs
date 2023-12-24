using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "list-fhir-import-jobs")]
public record AwsHealthlakeListFhirImportJobsOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--job-status")]
    public string? JobStatus { get; set; }

    [CommandSwitch("--submitted-before")]
    public long? SubmittedBefore { get; set; }

    [CommandSwitch("--submitted-after")]
    public long? SubmittedAfter { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}