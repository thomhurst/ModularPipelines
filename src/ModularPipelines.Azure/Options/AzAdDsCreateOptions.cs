using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "ds", "create")]
public record AzAdDsCreateOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--replica-sets")] string ReplicaSets,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--domain-config-type")]
    public string? DomainConfigType { get; set; }

    [CommandSwitch("--external-access")]
    public string? ExternalAccess { get; set; }

    [CommandSwitch("--filtered-sync")]
    public string? FilteredSync { get; set; }

    [CommandSwitch("--ldaps")]
    public string? Ldaps { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--notify-dc-admins")]
    public string? NotifyDcAdmins { get; set; }

    [CommandSwitch("--notify-global-admins")]
    public string? NotifyGlobalAdmins { get; set; }

    [CommandSwitch("--notify-others")]
    public string? NotifyOthers { get; set; }

    [CommandSwitch("--ntlm-v1")]
    public string? NtlmV1 { get; set; }

    [CommandSwitch("--pfx-cert")]
    public string? PfxCert { get; set; }

    [CommandSwitch("--pfx-cert-pwd")]
    public string? PfxCertPwd { get; set; }

    [CommandSwitch("--resource-forest")]
    public string? ResourceForest { get; set; }

    [CommandSwitch("--settings")]
    public string? Settings { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--sync-kerberos-pwd")]
    public string? SyncKerberosPwd { get; set; }

    [CommandSwitch("--sync-ntlm-pwd")]
    public string? SyncNtlmPwd { get; set; }

    [CommandSwitch("--sync-on-prem-pwd")]
    public string? SyncOnPremPwd { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tls-v1")]
    public string? TlsV1 { get; set; }
}