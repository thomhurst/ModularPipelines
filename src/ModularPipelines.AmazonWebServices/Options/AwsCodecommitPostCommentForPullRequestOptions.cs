using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "post-comment-for-pull-request")]
public record AwsCodecommitPostCommentForPullRequestOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId,
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--before-commit-id")] string BeforeCommitId,
[property: CommandSwitch("--after-commit-id")] string AfterCommitId,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}