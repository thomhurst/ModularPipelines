using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "public-ip", "prefix", "create")]
public record AzNetworkPublicIpPrefixCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-ip-prefix-name")]
    public string? CustomIpPrefixName { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--ip-tags")]
    public string? IpTags { get; set; }

    [CliOption("--length")]
    public string? Length { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}