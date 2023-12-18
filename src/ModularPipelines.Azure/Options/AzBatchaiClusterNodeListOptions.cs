using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batchai", "cluster", "node", "list")]
public record AzBatchaiClusterNodeListOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace")] string Workspace
) : AzOptions
{
}

