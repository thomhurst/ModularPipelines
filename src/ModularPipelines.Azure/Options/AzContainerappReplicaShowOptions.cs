using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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