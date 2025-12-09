using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecatalyst", "get-workflow")]
public record AwsCodecatalystGetWorkflowOptions(
[property: CliOption("--space-name")] string SpaceName,
[property: CliOption("--id")] string Id,
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}