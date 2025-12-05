using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "list-batch-job-executions")]
public record AwsM2ListBatchJobExecutionsOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--execution-ids")]
    public string[]? ExecutionIds { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--started-after")]
    public long? StartedAfter { get; set; }

    [CliOption("--started-before")]
    public long? StartedBefore { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}