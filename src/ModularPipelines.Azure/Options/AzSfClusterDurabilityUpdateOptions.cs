using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "durability", "update")]
public record AzSfClusterDurabilityUpdateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--durability-level")] string DurabilityLevel,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}