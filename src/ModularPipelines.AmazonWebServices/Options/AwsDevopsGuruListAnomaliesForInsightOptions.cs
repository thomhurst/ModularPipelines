using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "list-anomalies-for-insight")]
public record AwsDevopsGuruListAnomaliesForInsightOptions(
[property: CliOption("--insight-id")] string InsightId
) : AwsOptions
{
    [CliOption("--start-time-range")]
    public string? StartTimeRange { get; set; }

    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}