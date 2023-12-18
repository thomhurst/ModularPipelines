using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "create")]
public record AzEventhubsEventhubCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
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

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CommandSwitch("--partition-count")]
    public int? PartitionCount { get; set; }

    [CommandSwitch("--retention-time")]
    public string? RetentionTime { get; set; }

    [BooleanCommandSwitch("--skip-empty-archives")]
    public bool? SkipEmptyArchives { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tombstone-retention-time-in-hours")]
    public string? TombstoneRetentionTimeInHours { get; set; }
}