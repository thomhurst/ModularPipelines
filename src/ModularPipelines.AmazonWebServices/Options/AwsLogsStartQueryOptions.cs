using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "start-query")]
public record AwsLogsStartQueryOptions(
[property: CliOption("--start-time")] long StartTime,
[property: CliOption("--end-time")] long EndTime,
[property: CliOption("--query-string")] string QueryString
) : AwsOptions
{
    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--log-group-names")]
    public string[]? LogGroupNames { get; set; }

    [CliOption("--log-group-identifiers")]
    public string[]? LogGroupIdentifiers { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}