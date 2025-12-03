using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "update-component-type")]
public record AwsIottwinmakerUpdateComponentTypeOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--component-type-id")] string ComponentTypeId
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--property-definitions")]
    public IEnumerable<KeyValue>? PropertyDefinitions { get; set; }

    [CliOption("--extends-from")]
    public string[]? ExtendsFrom { get; set; }

    [CliOption("--functions")]
    public IEnumerable<KeyValue>? Functions { get; set; }

    [CliOption("--property-groups")]
    public IEnumerable<KeyValue>? PropertyGroups { get; set; }

    [CliOption("--component-type-name")]
    public string? ComponentTypeName { get; set; }

    [CliOption("--composite-component-types")]
    public IEnumerable<KeyValue>? CompositeComponentTypes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}