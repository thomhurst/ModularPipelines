using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "put-file")]
public record AwsCodecommitPutFileOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--branch-name")] string BranchName,
[property: CommandSwitch("--file-content")] string FileContent,
[property: CommandSwitch("--file-path")] string FilePath
) : AwsOptions
{
    [CommandSwitch("--file-mode")]
    public string? FileMode { get; set; }

    [CommandSwitch("--parent-commit-id")]
    public string? ParentCommitId { get; set; }

    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}