using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "db", "audit-policy", "update")]
public record AzSqlDbAuditPolicyUpdateOptions : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--blob-storage-target-state")]
    public string? BlobStorageTargetState { get; set; }

    [CliOption("--eh")]
    public string? Eh { get; set; }

    [CliOption("--ehari")]
    public string? Ehari { get; set; }

    [CliOption("--ehts")]
    public string? Ehts { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lats")]
    public string? Lats { get; set; }

    [CliOption("--lawri")]
    public string? Lawri { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-days")]
    public string? RetentionDays { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

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
}