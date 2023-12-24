using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-pull-request-title")]
public record AwsCodecommitUpdatePullRequestTitleOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--title")] string Title
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}