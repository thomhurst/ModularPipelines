using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ad", "ds", "create")]
public record AzAdDsCreateOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name,
[property: CliOption("--replica-sets")] string ReplicaSets,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--domain-config-type")]
    public string? DomainConfigType { get; set; }

    [CliOption("--external-access")]
    public string? ExternalAccess { get; set; }

    [CliOption("--filtered-sync")]
    public string? FilteredSync { get; set; }

    [CliOption("--ldaps")]
    public string? Ldaps { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--notify-dc-admins")]
    public string? NotifyDcAdmins { get; set; }

    [CliOption("--notify-global-admins")]
    public string? NotifyGlobalAdmins { get; set; }

    [CliOption("--notify-others")]
    public string? NotifyOthers { get; set; }

    [CliOption("--ntlm-v1")]
    public string? NtlmV1 { get; set; }

    [CliOption("--pfx-cert")]
    public string? PfxCert { get; set; }

    [CliOption("--pfx-cert-pwd")]
    public string? PfxCertPwd { get; set; }

    [CliOption("--resource-forest")]
    public string? ResourceForest { get; set; }

    [CliOption("--settings")]
    public string? Settings { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--sync-kerberos-pwd")]
    public string? SyncKerberosPwd { get; set; }

    [CliOption("--sync-ntlm-pwd")]
    public string? SyncNtlmPwd { get; set; }

    [CliOption("--sync-on-prem-pwd")]
    public string? SyncOnPremPwd { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tls-v1")]
    public string? TlsV1 { get; set; }
}