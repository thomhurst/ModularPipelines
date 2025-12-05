using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "put-object")]
public record AwsBackupstoragePutObjectOptions(
[property: CliOption("--backup-job-id")] string BackupJobId,
[property: CliOption("--object-name")] string ObjectName
) : AwsOptions
{
    [CliOption("--metadata-string")]
    public string? MetadataString { get; set; }

    [CliOption("--inline-chunk")]
    public string? InlineChunk { get; set; }

    [CliOption("--inline-chunk-length")]
    public long? InlineChunkLength { get; set; }

    [CliOption("--inline-chunk-checksum")]
    public string? InlineChunkChecksum { get; set; }

    [CliOption("--inline-chunk-checksum-algorithm")]
    public string? InlineChunkChecksumAlgorithm { get; set; }

    [CliOption("--object-checksum")]
    public string? ObjectChecksum { get; set; }

    [CliOption("--object-checksum-algorithm")]
    public string? ObjectChecksumAlgorithm { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}