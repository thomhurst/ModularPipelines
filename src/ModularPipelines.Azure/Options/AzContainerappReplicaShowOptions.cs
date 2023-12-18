using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "replica", "show")]
public record AzContainerappReplicaShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--replica")] string Replica,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--revision")]
    public string? Revision { get; set; }
}