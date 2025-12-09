using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-default-branch")]
public record AwsCodecommitUpdateDefaultBranchOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--default-branch-name")] string DefaultBranchName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}