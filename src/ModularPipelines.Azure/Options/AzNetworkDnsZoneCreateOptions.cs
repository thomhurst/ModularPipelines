using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "dns", "zone", "create")]
public record AzNetworkDnsZoneCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CommandSwitch("--parent-name")]
    public string? ParentName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

