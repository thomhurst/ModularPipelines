using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "schema-registry", "create")]
public record AzEventhubsNamespaceSchemaRegistryCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--group-properties")]
    public string? GroupProperties { get; set; }

    [CliOption("--schema-compatibility")]
    public string? SchemaCompatibility { get; set; }

    [CliOption("--schema-type")]
    public string? SchemaType { get; set; }
}