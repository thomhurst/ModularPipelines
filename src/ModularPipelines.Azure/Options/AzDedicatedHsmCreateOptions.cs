using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dedicated-hsm", "create")]
public record AzDedicatedHsmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mgmt-network-interfaces")]
    public string? MgmtNetworkInterfaces { get; set; }

    [CliOption("--mgmt-network-subnet")]
    public string? MgmtNetworkSubnet { get; set; }

    [CliOption("--network-interfaces")]
    public string? NetworkInterfaces { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--stamp-id")]
    public string? StampId { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}