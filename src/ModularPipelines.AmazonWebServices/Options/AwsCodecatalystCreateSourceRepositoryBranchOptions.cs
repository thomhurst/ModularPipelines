using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "create-source-repository-branch")]
public record AwsCodecatalystCreateSourceRepositoryBranchOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--source-repository-name")] string SourceRepositoryName,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--head-commit-id")]
    public string? HeadCommitId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}