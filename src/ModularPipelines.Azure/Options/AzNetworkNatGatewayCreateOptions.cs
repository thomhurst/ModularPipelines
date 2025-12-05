using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "nat", "gateway", "create")]
public record AzNetworkNatGatewayCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--idle-timeout")]
    public string? IdleTimeout { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--public-ip-addresses")]
    public string? PublicIpAddresses { get; set; }

    [CliOption("--public-ip-prefixes")]
    public string? PublicIpPrefixes { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}