using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "post-comment-for-compared-commit")]
public record AwsCodecommitPostCommentForComparedCommitOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--after-commit-id")] string AfterCommitId,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}