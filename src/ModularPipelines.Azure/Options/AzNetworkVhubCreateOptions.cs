using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "vhub", "create")]
public record AzNetworkVhubCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [BooleanCommandSwitch("--allow-b2b-traffic")]
    public bool? AllowB2bTraffic { get; set; }

    [CommandSwitch("--asn")]
    public string? Asn { get; set; }

    [CommandSwitch("--auto-scale-config")]
    public string? AutoScaleConfig { get; set; }

    [CommandSwitch("--hub-routing-preference")]
    public string? HubRoutingPreference { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vwan")]
    public string? Vwan { get; set; }
}

