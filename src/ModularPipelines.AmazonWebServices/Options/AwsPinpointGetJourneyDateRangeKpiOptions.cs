using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-journey-date-range-kpi")]
public record AwsPinpointGetJourneyDateRangeKpiOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--journey-id")] string JourneyId,
[property: CliOption("--kpi-name")] string KpiName
) : AwsOptions
{
    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}