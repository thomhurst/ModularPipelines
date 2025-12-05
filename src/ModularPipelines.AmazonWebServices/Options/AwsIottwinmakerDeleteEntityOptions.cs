using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "delete-entity")]
public record AwsIottwinmakerDeleteEntityOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}