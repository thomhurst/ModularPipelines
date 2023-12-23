using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "notify-object-complete")]
public record AwsBackupstorageNotifyObjectCompleteOptions(
[property: CommandSwitch("--backup-job-id")] string BackupJobId,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--object-checksum")] string ObjectChecksum,
[property: CommandSwitch("--object-checksum-algorithm")] string ObjectChecksumAlgorithm
) : AwsOptions
{
    [CommandSwitch("--metadata-string")]
    public string? MetadataString { get; set; }

    [CommandSwitch("--metadata-blob")]
    public string? MetadataBlob { get; set; }

    [CommandSwitch("--metadata-blob-length")]
    public long? MetadataBlobLength { get; set; }

    [CommandSwitch("--metadata-blob-checksum")]
    public string? MetadataBlobChecksum { get; set; }

    [CommandSwitch("--metadata-blob-checksum-algorithm")]
    public string? MetadataBlobChecksumAlgorithm { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}