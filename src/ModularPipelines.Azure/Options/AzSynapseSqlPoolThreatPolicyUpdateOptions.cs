using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("synapse", "sql", "pool", "threat-policy", "update")]
public record AzSynapseSqlPoolThreatPolicyUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--disabled-alerts")]
    public bool? DisabledAlerts { get; set; }

    [CliFlag("--email-account-admins")]
    public bool? EmailAccountAdmins { get; set; }

    [CliOption("--email-addresses")]
    public string? EmailAddresses { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--security-alert-policy-name")]
    public string? SecurityAlertPolicyName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--storage-endpoint")]
    public string? StorageEndpoint { get; set; }

    [CliOption("--storage-key")]
    public string? StorageKey { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}