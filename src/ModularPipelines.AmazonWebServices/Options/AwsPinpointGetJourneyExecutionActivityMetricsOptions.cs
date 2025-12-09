using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-journey-execution-activity-metrics")]
public record AwsPinpointGetJourneyExecutionActivityMetricsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--journey-activity-id")] string JourneyActivityId,
[property: CliOption("--journey-id")] string JourneyId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--page-size")]
    public string? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}