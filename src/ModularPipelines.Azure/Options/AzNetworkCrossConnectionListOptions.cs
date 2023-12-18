using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "cross-connection", "list")]
public record AzNetworkCrossConnectionListOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--peering-name")] string PeeringName
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

