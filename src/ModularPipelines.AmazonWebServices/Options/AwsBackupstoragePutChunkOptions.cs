using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "put-chunk")]
public record AwsBackupstoragePutChunkOptions(
[property: CliOption("--backup-job-id")] string BackupJobId,
[property: CliOption("--upload-id")] string UploadId,
[property: CliOption("--chunk-index")] long ChunkIndex,
[property: CliOption("--data")] string Data,
[property: CliOption("--length")] long Length,
[property: CliOption("--checksum")] string Checksum,
[property: CliOption("--checksum-algorithm")] string ChecksumAlgorithm
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}