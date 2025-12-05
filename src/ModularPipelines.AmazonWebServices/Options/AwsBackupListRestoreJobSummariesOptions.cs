using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-restore-job-summaries")]
public record AwsBackupListRestoreJobSummariesOptions : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--aggregation-period")]
    public string? AggregationPeriod { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}