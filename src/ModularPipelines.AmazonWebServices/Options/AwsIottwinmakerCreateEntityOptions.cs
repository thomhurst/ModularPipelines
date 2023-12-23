using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "create-entity")]
public record AwsIottwinmakerCreateEntityOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--entity-name")] string EntityName
) : AwsOptions
{
    [CommandSwitch("--entity-id")]
    public string? EntityId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--components")]
    public IEnumerable<KeyValue>? Components { get; set; }

    [CommandSwitch("--composite-components")]
    public IEnumerable<KeyValue>? CompositeComponents { get; set; }

    [CommandSwitch("--parent-entity-id")]
    public string? ParentEntityId { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}