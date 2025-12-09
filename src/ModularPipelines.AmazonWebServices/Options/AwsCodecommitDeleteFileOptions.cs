using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "delete-file")]
public record AwsCodecommitDeleteFileOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--branch-name")] string BranchName,
[property: CliOption("--file-path")] string FilePath,
[property: CliOption("--parent-commit-id")] string ParentCommitId
) : AwsOptions
{
    [CliOption("--commit-message")]
    public string? CommitMessage { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}