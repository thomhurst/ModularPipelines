using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-file")]
public record AwsCodecommitGetFileOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--file-path")] string FilePath
) : AwsOptions
{
    [CliOption("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}