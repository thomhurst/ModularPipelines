using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dls", "account", "update")]
public record AzDlsAccountUpdateOptions : AzOptions
{
    [CliOption("--account")]
    public int? Account { get; set; }

    [CliFlag("--allow-azure-ips")]
    public bool? AllowAzureIps { get; set; }

    [CliOption("--default-group")]
    public string? DefaultGroup { get; set; }

    [CliOption("--firewall-state")]
    public string? FirewallState { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-version")]
    public string? KeyVersion { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--trusted-id-provider-state")]
    public string? TrustedIdProviderState { get; set; }
}