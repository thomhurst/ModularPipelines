using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "delete-branch")]
public record AwsCodecommitDeleteBranchOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--branch-name")] string BranchName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}