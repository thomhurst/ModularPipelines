using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight-on-aks", "cluster", "node-profile", "create")]
public record AzHdinsightOnAksClusterNodeProfileCreateOptions(
[property: CommandSwitch("--count")] int Count,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--vm-size")] string VmSize
) : AzOptions
{
}

