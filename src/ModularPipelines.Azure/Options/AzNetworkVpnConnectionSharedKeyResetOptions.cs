using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vpn-connection", "shared-key", "reset")]
public record AzNetworkVpnConnectionSharedKeyResetOptions(
[property: CliOption("--key-length")] string KeyLength
) : AzOptions
{
    [CliOption("--connection-name")]
    public string? ConnectionName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}