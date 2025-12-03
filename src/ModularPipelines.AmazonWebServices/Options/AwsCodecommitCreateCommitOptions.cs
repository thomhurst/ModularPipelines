using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "create-commit")]
public record AwsCodecommitCreateCommitOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--parent-commit-id")]
    public string? ParentCommitId { get; set; }

    [CliOption("--author-name")]
    public string? AuthorName { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--put-files")]
    public string[]? PutFiles { get; set; }

    [CliOption("--delete-files")]
    public string[]? DeleteFiles { get; set; }

    [CliOption("--set-file-modes")]
    public string[]? SetFileModes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}