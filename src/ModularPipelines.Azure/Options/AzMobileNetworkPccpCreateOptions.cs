using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "pccp", "create")]
public record AzMobileNetworkPccpCreateOptions(
[property: CliOption("--access-interface")] string AccessInterface,
[property: CliOption("--local-diagnostics")] string LocalDiagnostics,
[property: CliOption("--name")] string Name,
[property: CliOption("--platform")] string Platform,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sites")] string Sites,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--core-network-tec")]
    public string? CoreNetworkTec { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ue-mtu")]
    public string? UeMtu { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}