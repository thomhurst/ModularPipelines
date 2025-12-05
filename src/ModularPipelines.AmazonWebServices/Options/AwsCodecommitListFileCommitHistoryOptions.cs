using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "list-file-commit-history")]
public record AwsCodecommitListFileCommitHistoryOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--file-path")] string FilePath
) : AwsOptions
{
    [CliOption("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}