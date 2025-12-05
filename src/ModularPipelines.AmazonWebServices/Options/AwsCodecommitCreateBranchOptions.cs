using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "create-branch")]
public record AwsCodecommitCreateBranchOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--branch-name")] string BranchName,
[property: CliOption("--commit-id")] string CommitId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}