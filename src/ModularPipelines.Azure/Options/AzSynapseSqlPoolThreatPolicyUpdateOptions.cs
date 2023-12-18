using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql", "pool", "threat-policy", "update")]
public record AzSynapseSqlPoolThreatPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--disabled-alerts")]
    public bool? DisabledAlerts { get; set; }

    [BooleanCommandSwitch("--email-account-admins")]
    public bool? EmailAccountAdmins { get; set; }

    [CommandSwitch("--email-addresses")]
    public string? EmailAddresses { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--security-alert-policy-name")]
    public string? SecurityAlertPolicyName { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--storage-endpoint")]
    public string? StorageEndpoint { get; set; }

    [CommandSwitch("--storage-key")]
    public string? StorageKey { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}