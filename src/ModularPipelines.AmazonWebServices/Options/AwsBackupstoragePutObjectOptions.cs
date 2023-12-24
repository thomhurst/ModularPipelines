using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "put-object")]
public record AwsBackupstoragePutObjectOptions(
[property: CommandSwitch("--backup-job-id")] string BackupJobId,
[property: CommandSwitch("--object-name")] string ObjectName
) : AwsOptions
{
    [CommandSwitch("--metadata-string")]
    public string? MetadataString { get; set; }

    [CommandSwitch("--inline-chunk")]
    public string? InlineChunk { get; set; }

    [CommandSwitch("--inline-chunk-length")]
    public long? InlineChunkLength { get; set; }

    [CommandSwitch("--inline-chunk-checksum")]
    public string? InlineChunkChecksum { get; set; }

    [CommandSwitch("--inline-chunk-checksum-algorithm")]
    public string? InlineChunkChecksumAlgorithm { get; set; }

    [CommandSwitch("--object-checksum")]
    public string? ObjectChecksum { get; set; }

    [CommandSwitch("--object-checksum-algorithm")]
    public string? ObjectChecksumAlgorithm { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}