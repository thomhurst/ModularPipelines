using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "baremetalmachine", "replace")]
public record AzNetworkcloudBaremetalmachineReplaceOptions(
[property: CommandSwitch("--limit-time-seconds")] string LimitTimeSeconds,
[property: CommandSwitch("--script")] string Script
) : AzOptions
{
    [CommandSwitch("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [CommandSwitch("--bmc-credentials")]
    public string? BmcCredentials { get; set; }

    [CommandSwitch("--bmc-mac-address")]
    public string? BmcMacAddress { get; set; }

    [CommandSwitch("--boot-mac-address")]
    public string? BootMacAddress { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--serial-number")]
    public string? SerialNumber { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

