using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-journey-run-execution-metrics")]
public record AwsPinpointGetJourneyRunExecutionMetricsOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--journey-id")] string JourneyId,
[property: CommandSwitch("--run-id")] string RunId
) : AwsOptions
{
    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}