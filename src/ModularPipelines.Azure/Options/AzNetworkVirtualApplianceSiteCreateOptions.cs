using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "virtual-appliance", "site", "create")]
public record AzNetworkVirtualApplianceSiteCreateOptions(
[property: CommandSwitch("--appliance-name")] string ApplianceName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [BooleanCommandSwitch("--allow")]
    public bool? Allow { get; set; }

    [BooleanCommandSwitch("--default")]
    public bool? Default { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--optimize")]
    public bool? Optimize { get; set; }
}

