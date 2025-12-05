using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "put-file")]
public record AwsCodecommitPutFileOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--branch-name")] string BranchName,
[property: CliOption("--file-content")] string FileContent,
[property: CliOption("--file-path")] string FilePath
) : AwsOptions
{
    [CliOption("--file-mode")]
    public string? FileMode { get; set; }

    [CliOption("--parent-commit-id")]
    public string? ParentCommitId { get; set; }

    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}