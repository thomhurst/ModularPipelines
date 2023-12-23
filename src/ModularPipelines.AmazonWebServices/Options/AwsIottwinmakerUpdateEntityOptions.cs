using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "update-entity")]
public record AwsIottwinmakerUpdateEntityOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--entity-id")] string EntityId
) : AwsOptions
{
    [CommandSwitch("--entity-name")]
    public string? EntityName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--component-updates")]
    public IEnumerable<KeyValue>? ComponentUpdates { get; set; }

    [CommandSwitch("--composite-component-updates")]
    public IEnumerable<KeyValue>? CompositeComponentUpdates { get; set; }

    [CommandSwitch("--parent-entity-update")]
    public string? ParentEntityUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}