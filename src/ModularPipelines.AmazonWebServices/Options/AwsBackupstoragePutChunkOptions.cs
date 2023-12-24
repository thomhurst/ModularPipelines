using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "put-chunk")]
public record AwsBackupstoragePutChunkOptions(
[property: CommandSwitch("--backup-job-id")] string BackupJobId,
[property: CommandSwitch("--upload-id")] string UploadId,
[property: CommandSwitch("--chunk-index")] long ChunkIndex,
[property: CommandSwitch("--data")] string Data,
[property: CommandSwitch("--length")] long Length,
[property: CommandSwitch("--checksum")] string Checksum,
[property: CommandSwitch("--checksum-algorithm")] string ChecksumAlgorithm
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}