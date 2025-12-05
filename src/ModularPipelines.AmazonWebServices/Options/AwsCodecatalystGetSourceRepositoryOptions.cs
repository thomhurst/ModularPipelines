using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "get-source-repository")]
public record AwsCodecatalystGetSourceRepositoryOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}