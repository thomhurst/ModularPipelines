using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sf", "cluster", "upgrade-type", "set")]
public record AzSfClusterUpgradeTypeSetOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--upgrade-mode")] string UpgradeMode
) : AzOptions
{
    [CommandSwitch("--version")]
    public string? Version { get; set; }
}