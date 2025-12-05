using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "notify-object-complete")]
public record AwsBackupstorageNotifyObjectCompleteOptions(
[property: CliOption("--backup-job-id")] string BackupJobId,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--object-checksum")] string ObjectChecksum,
[property: CliOption("--object-checksum-algorithm")] string ObjectChecksumAlgorithm
) : AwsOptions
{
    [CliOption("--metadata-string")]
    public string? MetadataString { get; set; }

    [CliOption("--metadata-blob")]
    public string? MetadataBlob { get; set; }

    [CliOption("--metadata-blob-length")]
    public long? MetadataBlobLength { get; set; }

    [CliOption("--metadata-blob-checksum")]
    public string? MetadataBlobChecksum { get; set; }

    [CliOption("--metadata-blob-checksum-algorithm")]
    public string? MetadataBlobChecksumAlgorithm { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}