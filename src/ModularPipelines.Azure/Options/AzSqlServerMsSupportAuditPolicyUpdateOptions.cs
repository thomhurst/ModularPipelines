using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "ms-support", "audit-policy", "update")]
public record AzSqlServerMsSupportAuditPolicyUpdateOptions : AzOptions
{
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

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

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

