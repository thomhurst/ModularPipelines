using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "describe-pull-request-events")]
public record AwsCodecommitDescribePullRequestEventsOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId
) : AwsOptions
{
    [CommandSwitch("--pull-request-event-type")]
    public string? PullRequestEventType { get; set; }

    [CommandSwitch("--actor-arn")]
    public string? ActorArn { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}