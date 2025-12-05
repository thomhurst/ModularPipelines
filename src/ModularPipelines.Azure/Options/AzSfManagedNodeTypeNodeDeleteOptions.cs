using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sf", "managed-node-type", "node", "delete")]
public record AzSfManagedNodeTypeNodeDeleteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--node-name")] string NodeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}