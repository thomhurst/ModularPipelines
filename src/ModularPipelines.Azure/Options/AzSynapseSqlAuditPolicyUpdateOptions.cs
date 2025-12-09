using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "sql", "audit-policy", "update")]
public record AzSynapseSqlAuditPolicyUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--blob-auditing-policy-name")]
    public string? BlobAuditingPolicyName { get; set; }

    [CliOption("--blob-storage-target-state")]
    public string? BlobStorageTargetState { get; set; }

    [CliOption("--eh")]
    public string? Eh { get; set; }

    [CliOption("--ehari")]
    public string? Ehari { get; set; }

    [CliOption("--ehts")]
    public string? Ehts { get; set; }

    [CliFlag("--enable-azure-monitor")]
    public bool? EnableAzureMonitor { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--lats")]
    public string? Lats { get; set; }

    [CliOption("--lawri")]
    public string? Lawri { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--queue-delay-milliseconds")]
    public string? QueueDelayMilliseconds { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

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

    [CliOption("--storage-subscription")]
    public string? StorageSubscription { get; set; }

    [CliFlag("--use-secondary-key")]
    public bool? UseSecondaryKey { get; set; }
}