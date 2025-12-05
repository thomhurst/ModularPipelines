using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "lb", "address-pool", "tunnel-interface", "add")]
public record AzNetworkLbAddressPoolTunnelInterfaceAddOptions(
[property: CliOption("--address-pool")] string AddressPool,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--lb-name")] string LbName,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--index")]
    public string? Index { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }
}