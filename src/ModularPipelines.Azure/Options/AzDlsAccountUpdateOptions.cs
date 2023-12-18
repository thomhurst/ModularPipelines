using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "account", "update")]
public record AzDlsAccountUpdateOptions : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [BooleanCommandSwitch("--allow-azure-ips")]
    public bool? AllowAzureIps { get; set; }

    [CommandSwitch("--default-group")]
    public string? DefaultGroup { get; set; }

    [CommandSwitch("--firewall-state")]
    public string? FirewallState { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--key-version")]
    public string? KeyVersion { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--trusted-id-provider-state")]
    public string? TrustedIdProviderState { get; set; }
}

