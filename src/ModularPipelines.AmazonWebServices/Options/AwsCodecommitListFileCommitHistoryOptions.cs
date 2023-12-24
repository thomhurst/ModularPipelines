using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "list-file-commit-history")]
public record AwsCodecommitListFileCommitHistoryOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--file-path")] string FilePath
) : AwsOptions
{
    [CommandSwitch("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}