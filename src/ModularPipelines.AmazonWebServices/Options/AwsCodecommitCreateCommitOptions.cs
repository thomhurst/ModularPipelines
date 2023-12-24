using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "create-commit")]
public record AwsCodecommitCreateCommitOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--branch-name")] string BranchName
) : AwsOptions
{
    [CommandSwitch("--parent-commit-id")]
    public string? ParentCommitId { get; set; }

    [CommandSwitch("--author-name")]
    public string? AuthorName { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--put-files")]
    public string[]? PutFiles { get; set; }

    [CommandSwitch("--delete-files")]
    public string[]? DeleteFiles { get; set; }

    [CommandSwitch("--set-file-modes")]
    public string[]? SetFileModes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}