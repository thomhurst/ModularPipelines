using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "get-query-results")]
public record AwsCloudtrailGetQueryResultsOptions(
[property: CommandSwitch("--query-id")] string QueryId
) : AwsOptions
{
    [CommandSwitch("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-query-results")]
    public int? MaxQueryResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}