using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "get-chunk")]
public record AwsBackupstorageGetChunkOptions(
[property: CliOption("--storage-job-id")] string StorageJobId,
[property: CliOption("--chunk-token")] string ChunkToken
) : AwsOptions;