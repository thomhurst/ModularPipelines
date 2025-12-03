using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "pccp", "update")]
public record AzMobileNetworkPccpUpdateOptions : AzOptions
{
    [CliOption("--access-interface")]
    public string? AccessInterface { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--core-network-tec")]
    public string? CoreNetworkTec { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--local-diagnostics")]
    public string? LocalDiagnostics { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sites")]
    public string? Sites { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--ue-mtu")]
    public string? UeMtu { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}