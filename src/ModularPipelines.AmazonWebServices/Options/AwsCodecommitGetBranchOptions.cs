using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "get-branch")]
public record AwsCodecommitGetBranchOptions : AwsOptions
{
    [CliOption("--repository-name")]
    public string? RepositoryName { get; set; }

    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}