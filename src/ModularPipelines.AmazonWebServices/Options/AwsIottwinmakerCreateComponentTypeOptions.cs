using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "create-component-type")]
public record AwsIottwinmakerCreateComponentTypeOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--component-type-id")] string ComponentTypeId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--property-definitions")]
    public IEnumerable<KeyValue>? PropertyDefinitions { get; set; }

    [CommandSwitch("--extends-from")]
    public string[]? ExtendsFrom { get; set; }

    [CommandSwitch("--functions")]
    public IEnumerable<KeyValue>? Functions { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--property-groups")]
    public IEnumerable<KeyValue>? PropertyGroups { get; set; }

    [CommandSwitch("--component-type-name")]
    public string? ComponentTypeName { get; set; }

    [CommandSwitch("--composite-component-types")]
    public IEnumerable<KeyValue>? CompositeComponentTypes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}