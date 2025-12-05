using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "get-query-results")]
public record AwsCloudtrailGetQueryResultsOptions(
[property: CliOption("--query-id")] string QueryId
) : AwsOptions
{
    [CliOption("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-query-results")]
    public int? MaxQueryResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}