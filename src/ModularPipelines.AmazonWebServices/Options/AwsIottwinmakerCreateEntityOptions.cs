using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "create-entity")]
public record AwsIottwinmakerCreateEntityOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--entity-name")] string EntityName
) : AwsOptions
{
    [CliOption("--entity-id")]
    public string? EntityId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--components")]
    public IEnumerable<KeyValue>? Components { get; set; }

    [CliOption("--composite-components")]
    public IEnumerable<KeyValue>? CompositeComponents { get; set; }

    [CliOption("--parent-entity-id")]
    public string? ParentEntityId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}