using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-folder")]
public record AwsCodecommitGetFolderOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--folder-path")] string FolderPath
) : AwsOptions
{
    [CliOption("--commit-specifier")]
    public string? CommitSpecifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}