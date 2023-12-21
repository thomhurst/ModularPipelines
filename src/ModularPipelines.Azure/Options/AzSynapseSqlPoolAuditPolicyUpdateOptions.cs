using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "sql", "pool", "audit-policy", "update")]
public record AzSynapseSqlPoolAuditPolicyUpdateOptions : AzOptions
{
    [CommandSwitch("--actions")]
    public string? Actions { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--blob-storage-target-state")]
    public string? BlobStorageTargetState { get; set; }

    [CommandSwitch("--eh")]
    public string? Eh { get; set; }

    [CommandSwitch("--ehari")]
    public string? Ehari { get; set; }

    [CommandSwitch("--ehts")]
    public string? Ehts { get; set; }

    [BooleanCommandSwitch("--enable-azure-monitor")]
    public bool? EnableAzureMonitor { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--lats")]
    public string? Lats { get; set; }

    [CommandSwitch("--lawri")]
    public string? Lawri { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

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

    [CommandSwitch("--storage-subscription")]
    public string? StorageSubscription { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [BooleanCommandSwitch("--use-secondary-key")]
    public bool? UseSecondaryKey { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}