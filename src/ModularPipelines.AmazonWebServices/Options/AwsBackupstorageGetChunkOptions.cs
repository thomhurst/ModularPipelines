using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backupstorage", "get-chunk")]
public record AwsBackupstorageGetChunkOptions(
[property: CommandSwitch("--storage-job-id")] string StorageJobId,
[property: CommandSwitch("--chunk-token")] string ChunkToken
) : AwsOptions;