using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "delete-dev-environment")]
public record AwsCodecatalystDeleteDevEnvironmentOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--project-name")] string ProjectName,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}