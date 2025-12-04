using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "eventhub", "update")]
public record AzEventhubsEventhubUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--archive-name-format")]
    public string? ArchiveNameFormat { get; set; }

    [CliOption("--blob-container")]
    public string? BlobContainer { get; set; }

    [CliOption("--capture-interval")]
    public string? CaptureInterval { get; set; }

    [CliOption("--capture-size-limit")]
    public string? CaptureSizeLimit { get; set; }

    [CliOption("--cleanup-policy")]
    public string? CleanupPolicy { get; set; }

    [CliOption("--destination-name")]
    public string? DestinationName { get; set; }

    [CliFlag("--enable-capture")]
    public bool? EnableCapture { get; set; }

    [CliOption("--encoding")]
    public string? Encoding { get; set; }

    [CliOption("--event-hub-name")]
    public string? EventHubName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--skip-empty-archives")]
    public bool? SkipEmptyArchives { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tombstone-retention-time-in-hours")]
    public string? TombstoneRetentionTimeInHours { get; set; }
}