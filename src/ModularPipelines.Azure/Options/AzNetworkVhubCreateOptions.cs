using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "vhub", "create")]
public record AzNetworkVhubCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliFlag("--allow-b2b-traffic")]
    public bool? AllowB2bTraffic { get; set; }

    [CliOption("--asn")]
    public string? Asn { get; set; }

    [CliOption("--auto-scale-config")]
    public string? AutoScaleConfig { get; set; }

    [CliOption("--hub-routing-preference")]
    public string? HubRoutingPreference { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vwan")]
    public string? Vwan { get; set; }
}