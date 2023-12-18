using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "threat-policy", "update")]
public record AzSqlDbThreatPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--disabled-alerts")]
    public bool? DisabledAlerts { get; set; }

    [CommandSwitch("--email-account-admins")]
    public int? EmailAccountAdmins { get; set; }

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

    [CommandSwitch("--server")]
    public string? Server { get; set; }

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
}

