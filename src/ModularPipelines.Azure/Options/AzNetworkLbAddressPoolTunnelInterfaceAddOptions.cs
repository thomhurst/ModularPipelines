using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "lb", "address-pool", "tunnel-interface", "add")]
public record AzNetworkLbAddressPoolTunnelInterfaceAddOptions(
[property: CommandSwitch("--address-pool")] string AddressPool,
[property: CommandSwitch("--identifier")] string Identifier,
[property: CommandSwitch("--lb-name")] string LbName,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }
}