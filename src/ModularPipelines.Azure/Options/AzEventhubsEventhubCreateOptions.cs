using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "eventhub", "create")]
public record AzEventhubsEventhubCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--partition-count")]
    public int? PartitionCount { get; set; }

    [CliOption("--retention-time")]
    public string? RetentionTime { get; set; }

    [CliFlag("--skip-empty-archives")]
    public bool? SkipEmptyArchives { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tombstone-retention-time-in-hours")]
    public string? TombstoneRetentionTimeInHours { get; set; }
}