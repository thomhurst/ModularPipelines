using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "get-comments-for-pull-request")]
public record AwsCodecommitGetCommentsForPullRequestOptions(
[property: CommandSwitch("--pull-request-id")] string PullRequestId
) : AwsOptions
{
    [CommandSwitch("--repository-name")]
    public string? RepositoryName { get; set; }

    [CommandSwitch("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CommandSwitch("--after-commit-id")]
    public string? AfterCommitId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}