using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "get-source-repository-clone-urls")]
public record AwsCodecatalystGetSourceRepositoryCloneUrlsOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--source-repository-name")] string SourceRepositoryName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}