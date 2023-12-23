using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "delete-file")]
public record AwsCodecommitDeleteFileOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--branch-name")] string BranchName,
[property: CommandSwitch("--file-path")] string FilePath,
[property: CommandSwitch("--parent-commit-id")] string ParentCommitId
) : AwsOptions
{
    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}