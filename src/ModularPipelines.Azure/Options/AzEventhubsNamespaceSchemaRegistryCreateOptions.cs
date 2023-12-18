using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "schema-registry", "create")]
public record AzEventhubsNamespaceSchemaRegistryCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--group-properties")]
    public string? GroupProperties { get; set; }

    [CommandSwitch("--schema-compatibility")]
    public string? SchemaCompatibility { get; set; }

    [CommandSwitch("--schema-type")]
    public string? SchemaType { get; set; }
}

