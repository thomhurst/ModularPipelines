using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "update")]
public record AzEventhubsEventhubUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--archive-name-format")]
    public string? ArchiveNameFormat { get; set; }

    [CommandSwitch("--blob-container")]
    public string? BlobContainer { get; set; }

    [CommandSwitch("--capture-interval")]
    public string? CaptureInterval { get; set; }

    [CommandSwitch("--capture-size-limit")]
    public string? CaptureSizeLimit { get; set; }

    [CommandSwitch("--cleanup-policy")]
    public string? CleanupPolicy { get; set; }

    [CommandSwitch("--destination-name")]
    public string? DestinationName { get; set; }

    [BooleanCommandSwitch("--enable-capture")]
    public bool? EnableCapture { get; set; }

    [CommandSwitch("--encoding")]
    public string? Encoding { get; set; }

    [CommandSwitch("--event-hub-name")]
    public string? EventHubName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--partition-count")]
    public int? PartitionCount { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [BooleanCommandSwitch("--skip-empty-archives")]
    public bool? SkipEmptyArchives { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tombstone-retention-time-in-hours")]
    public string? TombstoneRetentionTimeInHours { get; set; }
}