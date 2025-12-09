using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "post-comment-for-compared-commit")]
public record AwsCodecommitPostCommentForComparedCommitOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--after-commit-id")] string AfterCommitId,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--before-commit-id")]
    public string? BeforeCommitId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}