using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "update-entity")]
public record AwsIottwinmakerUpdateEntityOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--entity-name")]
    public string? EntityName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--component-updates")]
    public IEnumerable<KeyValue>? ComponentUpdates { get; set; }

    [CliOption("--composite-component-updates")]
    public IEnumerable<KeyValue>? CompositeComponentUpdates { get; set; }

    [CliOption("--parent-entity-update")]
    public string? ParentEntityUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}