using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "describe-pull-request-events")]
public record AwsCodecommitDescribePullRequestEventsOptions(
[property: CliOption("--pull-request-id")] string PullRequestId
) : AwsOptions
{
    [CliOption("--pull-request-event-type")]
    public string? PullRequestEventType { get; set; }

    [CliOption("--actor-arn")]
    public string? ActorArn { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}