using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces", "create-tags")]
public record AwsWorkspacesCreateTagsOptions(
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--tags")] string[] Tags
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}