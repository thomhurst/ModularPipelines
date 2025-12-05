using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "get-journey-execution-metrics")]
public record AwsPinpointGetJourneyExecutionMetricsOptions(
[property: CliOption("--application-id")] string ApplicationId,
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