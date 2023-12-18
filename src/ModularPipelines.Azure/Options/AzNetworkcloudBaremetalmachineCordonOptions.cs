using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "baremetalmachine", "cordon")]
public record AzNetworkcloudBaremetalmachineCordonOptions : AzOptions
{
    [CommandSwitch("--bare-metal-machine-name")]
    public string? BareMetalMachineName { get; set; }

    [BooleanCommandSwitch("--evacuate")]
    public bool? Evacuate { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}