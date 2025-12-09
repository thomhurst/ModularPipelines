using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-journey-run-execution-activity-metrics")]
public record AwsPinpointGetJourneyRunExecutionActivityMetricsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--journey-activity-id")] string JourneyActivityId,
[property: CliOption("--journey-id")] string JourneyId,
[property: CliOption("--run-id")] string RunId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}