using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "get-component-type")]
public record AwsIottwinmakerGetComponentTypeOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--component-type-id")] string ComponentTypeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}